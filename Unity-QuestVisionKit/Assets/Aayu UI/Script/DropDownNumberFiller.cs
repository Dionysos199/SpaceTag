using TMPro;
using UnityEngine;

public class DropdownNumberFiller : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    void Start()
    {
        FillDropdownWithNumbers();
    }

    private void FillDropdownWithNumbers()
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown reference is missing!");
            return;
        }

        dropdown.ClearOptions();  // Clear existing options

        // Create a list of numbers from 0 to 99
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i <= 99; i++)
        {
            options.Add(i.ToString());
        }

        // Add options to the dropdown
        dropdown.AddOptions(options);
    }
}
