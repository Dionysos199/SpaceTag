using UnityEngine;
using UnityEngine.UI;

public class CloseToggleHandler : MonoBehaviour
{
    [Header("Toggles to Close Panels")]
    public Toggle[] closeToggles; // Assign one per panel

    [Header("Objects to Disable on Close")]
    public GameObject[] objectsToDisable; // Must match toggle count

    [Header("Object to Enable on Close")]
    public GameObject objectToEnable; // Main menu or fallback UI

    private void Start()
    {
        for (int i = 0; i < closeToggles.Length; i++)
        {
            int index = i;
            closeToggles[i].onValueChanged.AddListener((isOn) => OnCloseToggle(index, isOn));
        }
    }

    void OnCloseToggle(int index, bool isOn)
    {
        if (isOn)
        {
            // Disable the target object at the same index
            if (objectsToDisable != null && index < objectsToDisable.Length && objectsToDisable[index] != null)
            {
                objectsToDisable[index].SetActive(false);
            }

            // Enable the shared fallback object
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            // Optionally reset the toggle to OFF
            closeToggles[index].isOn = false;
        }
    }
}
