using System;
using FSM;
using UnityEngine;

public class Walk : StateBase<NPCState>
{
    private Animator animator;
    private Transform[] waypoints;
    private Transform npcTransform;
    private Action<int> onCurrentIndexChanged;

    private int currentIndex = 0;
    private float speed;
    private Vector3 direction;

    public Walk(
        Animator animator, 
        Transform[] waypoints,
        Transform npcTransform, 
        Action<int> onCurrentIndexChanged,
        float speed,   
        bool needsExitTime) : base(needsExitTime)
    {
        this.animator = animator;
        this.waypoints = waypoints;
        this.npcTransform = npcTransform;
        this.speed = speed;
        this.onCurrentIndexChanged = onCurrentIndexChanged;
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Walk");
        direction = (waypoints[currentIndex].position - npcTransform.position).normalized;
        npcTransform.localScale = new Vector3(direction.x < 0 ? 1f : -1f, 1f, 1f);
    }

    public override void OnLogic()
    {
        
        npcTransform.Translate(direction * (Time.deltaTime * speed));
    }

    public override void OnExit()
    {
        currentIndex = (currentIndex + 1) % waypoints.Length;
        onCurrentIndexChanged?.Invoke(currentIndex);
    }
}