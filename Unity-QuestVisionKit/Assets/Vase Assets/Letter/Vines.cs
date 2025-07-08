using System.Collections;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Vines : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    
    void Start()
    {
        if (particles != null)
        {
            particles.Stop();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (particles != null)
        {
            particles.Play();
        }
    }
}
