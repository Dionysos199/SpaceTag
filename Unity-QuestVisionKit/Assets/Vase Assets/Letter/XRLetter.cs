using Oculus.Interaction.HandGrab;
using UnityEngine;

public class XRLetter : MonoBehaviour
{
    [SerializeField] private GameObject uiMenu;
    
    void Start()
    {
        uiMenu.SetActive(false);
        
        GetComponentInChildren<HandGrabInteractable>().WhenSelectingInteractorAdded.Action += ShowUI;
    }
    
    private void ShowUI(HandGrabInteractor interactor)
    {
        uiMenu.SetActive(true);
        uiMenu.GetComponent<Animator>().Play("UIBounce");
    }
}
