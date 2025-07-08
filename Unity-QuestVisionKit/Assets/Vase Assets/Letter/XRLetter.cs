using Oculus.Interaction.HandGrab;
using UnityEngine;

public class XRLetter : MonoBehaviour
{
    [SerializeField] private GameObject uiMenu;
    [SerializeField] private AnimationClip letterAnimation;
    
    private Animator letterAnimator;
    
    void Start()
    {
        uiMenu.SetActive(false);
        letterAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (letterAnimator.GetCurrentAnimatorStateInfo(0).IsName(letterAnimation.name))
        {
            if (letterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Invoke(nameof(ShowUI), 3f);
                this.enabled = false;
            }
        }
    }
    
    private void ShowUI()
    {
        uiMenu.SetActive(true);
        
        if (uiMenu.GetComponent<Animator>() != null)
        {
            uiMenu.GetComponent<Animator>().Play("UIBounce");
        }
    }
}
