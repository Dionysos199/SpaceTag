using UnityEngine;

public class LetterInsideGrab : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private AnimationClip letterAnimation;
    
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(letterAnimation.name))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                objectToActivate.SetActive(true);
                this.enabled = false; // Stop checking
            }
        }
    }
}
