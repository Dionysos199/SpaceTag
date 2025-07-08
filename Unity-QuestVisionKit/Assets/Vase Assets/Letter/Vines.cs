using System.Collections;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Vines : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    
    void Start()
    {
        GetComponentInChildren<HandGrabInteractable>().WhenSelectingInteractorAdded.Action += PlayParticles;
    }
    
    private void PlayParticles(HandGrabInteractor interactor)
    {
        if (particles != null)
        {
            particles.Play();
        }
    }
}
