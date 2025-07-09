using Oculus.Interaction.HandGrab;
using UnityEngine;

public class XRLetter : MonoBehaviour
{
    [SerializeField] private GameObject uiMenu;
    [SerializeField] private AnimationClip letterAnimation;
    [SerializeField] private AnimationClip uiBounceAnimation;
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
            if (letterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
            {
                Invoke(nameof(ShowUI), .2f);
                this.enabled = false;
            }
        }
    }
    
    private void ShowUI()
    {
        uiMenu.SetActive(true);
        
        if (uiMenu.GetComponent<Animator>() != null && uiBounceAnimation != null)
        {
            uiMenu.GetComponent<Animator>().Play("UIBounce");
        }
    }
}
