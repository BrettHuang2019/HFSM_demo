using FSM;
using UnityEngine;

public class Idle : StateBase<NPCState>
{
    private Animator animator;
    private float idleTime;
    private Timer timer;
    
    public Idle(Animator animator,float idleTime, bool needsExitTime) : base(needsExitTime)
    {
        this.animator = animator;
        this.idleTime = idleTime; 
        timer = new Timer();
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Idle");
        timer.Reset();
    }

    public override void OnLogic()
    {
        if (timer.Elapsed > idleTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExit()
    {
        animator.ResetTrigger("Idle");
    }
}