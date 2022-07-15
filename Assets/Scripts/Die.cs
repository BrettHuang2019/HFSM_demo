using FSM;
using UnityEngine;

public class Die : StateBase<SuperState>
{
    private Animator animator;
    
    public Die(Animator animator, bool needsExitTime) : base(needsExitTime)
    {
        this.animator = animator;
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Die");
    }

    public override void OnExit()
    {
        animator.ResetTrigger("Die");
    }
}