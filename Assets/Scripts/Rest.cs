using FSM;
using UnityEngine;

public class Rest : StateBase<NPCState>
{
    private Animator animator;
    private float restTime;
    private Timer timer;
    
    public Rest(Animator animator,float restTime, bool needsExitTime) : base(needsExitTime)
    {
        this.animator = animator;
        this.restTime = restTime; 
        timer = new Timer();
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Rest");
        timer.Reset();
    }

    public override void OnLogic()
    {
        if (timer.Elapsed > restTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExit()
    {
        animator.ResetTrigger("Rest");
    }
}