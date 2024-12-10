using UnityEngine;

public class PlayerAnimation : AnimationController
{
    private readonly int isWalking = Animator.StringToHash("isWalking");
    private readonly int Running = Animator.StringToHash("Running");
    private readonly int isHiding = Animator.StringToHash("isHiding");
    private readonly int die = Animator.StringToHash("die");

    private readonly float magnitudeThreshold = 0.5f;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

    }
    private void OnPlayerWalk(Vector2 vector)
    {
        animator.SetBool(isWalking, vector.magnitude > magnitudeThreshold);
    }

    private void OnPlayerRun()
    {
        animator.SetTrigger(Running);
    }

    private void OnPlayerHide()
    {
        animator.SetBool(isHiding, false);
    }

    private void OnPlayerDie()
    {
        animator.SetTrigger(die);
    }


}
