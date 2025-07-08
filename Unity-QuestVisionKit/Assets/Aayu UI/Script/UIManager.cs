using UnityEngine;
using UnityEngine.UI;

public class UIToggleManager : MonoBehaviour
{
    [Header("Object to Disable When Menu is Open")]
    public GameObject objectToDisable;
    
    [Header("UI Panels")]
    public GameObject[] uiPanels; // Assign 4 UI panels here

    [Header("UI Toggles")]
    public Toggle[] uiToggles; // Assign 4 toggles here

    
    private void Start()
    {
        for (int i = 0; i < uiToggles.Length; i++)
        {
            int index = i; // Capture index for closure
            uiToggles[i].onValueChanged.AddListener((isOn) => OnToggleChanged(index, isOn));
        }

        // Optional: Start with all panels closed
        CloseAllUIPanels();
    }

    void OnToggleChanged(int index, bool isOn)
    {
        if (isOn)
        {
            // Disable object when any menu is opened
            if (objectToDisable != null)
                objectToDisable.SetActive(false);
            
            // Turn off all other toggles and show the selected panel
            for (int i = 0; i < uiToggles.Length; i++)
            {
                bool toggleOn = (i == index);
                uiToggles[i].isOn = toggleOn;
                uiPanels[i].SetActive(toggleOn);
            }
        }
        else
        {
            // If toggled off manually, close its panel
            uiPanels[index].SetActive(false);
        }
    }

    void CloseAllUIPanels()
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }
}
