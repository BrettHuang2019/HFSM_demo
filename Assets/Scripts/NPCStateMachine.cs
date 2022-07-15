using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public enum NPCState
{
    IDLE,
    WALK,
    REST,
}

public enum SuperState
{
    NULL,
    DAY,
    NIGHT,
    NPCDIE
}

public class NPCStateMachine : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float idleTime;
    [SerializeField]private float restTime;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private SpriteRenderer restZone;

    private StateMachine<SuperState, string> fsm;
    private StateMachine<SuperState, NPCState, string> nightSuperState;
    private StateMachine<SuperState, NPCState, string> daySuperState;

    private int currentIndex = 0;

    private void Start()
    {
        // Day Super State
        daySuperState = new StateMachine<SuperState, NPCState, string>();

        daySuperState.AddState(NPCState.IDLE, new Idle(animator, idleTime, true));
        daySuperState.AddState(NPCState.WALK, new Walk(animator, waypoints, transform,OnCurrentIndexChanged, speed, false));
        
        daySuperState.AddTransition(NPCState.IDLE, NPCState.WALK);
        daySuperState.AddTransition(NPCState.WALK, NPCState.IDLE,
            transition => 
                Vector2.Distance(transform.position, waypoints[currentIndex].position) < 0.1f);
        
        
        // Night Super State
        nightSuperState = new StateMachine<SuperState, NPCState, string>();
        
        nightSuperState.AddState(NPCState.IDLE, new Idle(animator, idleTime, true));
        nightSuperState.AddState(NPCState.WALK, new Walk(animator, waypoints, transform,OnCurrentIndexChanged, speed, false));
        nightSuperState.AddState(NPCState.REST, new Rest(animator, restTime, true));


        nightSuperState.AddTransition(NPCState.IDLE, NPCState.WALK);
        nightSuperState.AddTransition(NPCState.REST, NPCState.WALK);
        
        nightSuperState.AddTransition(NPCState.WALK, NPCState.REST,
            transition => 
                Vector2.Distance(transform.position, waypoints[currentIndex].position) < 0.1f 
                && restZone.bounds.Contains(transform.position));
        
        nightSuperState.AddTransition(NPCState.WALK, NPCState.IDLE,
            transition => 
                Vector2.Distance(transform.position, waypoints[currentIndex].position) < 0.1f);
        
        // fsm init
        fsm = new StateMachine<SuperState, string>();
        
        fsm.AddState(SuperState.DAY, daySuperState);
        fsm.AddState(SuperState.NIGHT, nightSuperState);
        fsm.AddState(SuperState.NPCDIE, new Die(animator,false));

        fsm.AddTriggerTransition("Day", new Transition<SuperState>(SuperState.NIGHT, SuperState.DAY));
        fsm.AddTriggerTransition("Night", new Transition<SuperState>(SuperState.DAY, SuperState.NIGHT));
        fsm.AddTriggerTransitionFromAny("Die", new Transition<SuperState>(SuperState.NULL,SuperState.NPCDIE));


        fsm.Init();
    }

    private void Update()
    {
        fsm.OnLogic();
    }

    private void OnCurrentIndexChanged(int newIndex)
    {
        currentIndex = newIndex;
    }

    public void TriggerNight(bool isNight)
    {
        fsm.Trigger(isNight ? "Night" : "Day");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        fsm.Trigger("Die");
    }
}