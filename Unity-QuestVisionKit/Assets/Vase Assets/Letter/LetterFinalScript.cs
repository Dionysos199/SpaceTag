using UnityEngine;

public class LetterFinalScript : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToUnparent;
    [SerializeField] private float delayBeforeStart = 10f;
    [SerializeField] private float delayBeforeTrigger = 3f;
    
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Start the sequence after delay
        Invoke(nameof(StartFirstAnimation), delayBeforeStart);
    }
    
    private void StartFirstAnimation()
    {
        // First animation starts automatically (from Idle → LetterAnimation)
        // Wait for it to finish, then trigger second animation
        Invoke(nameof(TriggerSecondAnimation), delayBeforeTrigger);
    }
    
    private void TriggerSecondAnimation()
    {
        // Fire the trigger for your transition
        animator.SetTrigger("NextAnimation");
        
        // Activate object after second animation starts
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
        
        // Unparent object (making it independent with its children)
        if (objectToUnparent != null)
        {
            objectToUnparent.transform.SetParent(null);
        }
    }
}
