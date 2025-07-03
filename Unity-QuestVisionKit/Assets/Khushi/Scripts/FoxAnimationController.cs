using UnityEngine;

public class FoxAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void TriggerHug()
    {
        animator.SetTrigger("HugTrigger");
        Debug.Log("HugTrigger");
    }
}
