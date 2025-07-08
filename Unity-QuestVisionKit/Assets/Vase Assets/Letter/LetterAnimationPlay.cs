using Oculus.Interaction.HandGrab;
using UnityEngine;
using Oculus.Interaction.Input.Compatibility.OVR;

public class LetterAnimationPlay : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip;
    
    void Start()
    {
        Invoke(nameof(PlayAnimation), 10f);
    }
    
    private void PlayAnimation()
    {
        GetComponent<Animator>().Play(animationClip.name);
    }
}
