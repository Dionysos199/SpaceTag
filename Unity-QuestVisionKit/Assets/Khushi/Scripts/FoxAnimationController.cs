using UnityEngine;

public class FoxAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource yayAudioSource;

    public void TriggerHug()
    {
        animator.SetTrigger("HugTrigger");
        yayAudioSource.Play();
        Debug.Log("HugTrigger");
    }
}
