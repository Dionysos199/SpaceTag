using UnityEngine;

public class LetterAnimationPlay : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip;
    
    void Start()
    {
        GetComponent<Animator>().Play(animationClip.name);
    }
}
