using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    [SerializeField] Animator animator;

    public enum animType
    {
        Idle, Walk, TurnLeft, TurnRight, Jump, Push
    }
    public animType animationType;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            PlayAnim(animationType);
    }

    public void PlayAnim(animType type)
    {
        ClearTrigger();
        switch (type)
        {
            case animType.Idle:
                Idle();
                break;
            case animType.Walk:
                Walk();
                break;
            case animType.TurnLeft:
                Turn(true);
                break;
            case animType.TurnRight:
                Turn(false);
                break;
            case animType.Jump:
                Jump();
                break;
            case animType.Push:
                Push();
                break;
        }
    }

    void Idle()
    {
        animator.SetTrigger("Idle");
    }

    void Walk()
    {
        animator.SetTrigger("Walk");
    }

    void Turn(bool isLeft)
    {
        animator.SetBool("IsTurnLeft", isLeft);
        animator.SetTrigger("Turn");
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Push()
    {
        animator.SetTrigger("Push");
    }

    void ClearTrigger()
    {
        for (int i = 0; i < 3; i++) { }
        //animator.ResetTrigger(i);
    }
}
