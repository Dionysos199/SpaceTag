using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction.Samples;

public class DropdownPopulator : MonoBehaviour
{
    [SerializeField] private DropDownGroup dropDownGroup;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Transform togglesParent;
    [SerializeField] private Toggle togglePrefab;

    public void Populate(List<string> items)
    {
        Debug.Log($"[DropdownPopulator] Starting population with {items.Count} items.");

        //Clear();

        if (items.Count == 0)
        {
            Debug.LogWarning("[DropdownPopulator] No items to populate.");
            return;
        }
        Debug.Log("khawaltne ya zalame");
        Toggle[] toggles = new Toggle[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log("item"+i + " "+ items[i].ToString()); 
            Toggle toggle = Instantiate(togglePrefab, togglesParent);
            toggles[i] = toggle;

            DropdownUserItem item = toggle.GetComponent<DropdownUserItem>();
            if (item != null)
            {
                item.SetUsername(items[i]);
                Debug.Log($"[DropdownPopulator] Set username on toggle: {items[i]}");
            }
            else
            {
                Debug.LogWarning($"[DropdownPopulator] Toggle missing DropdownUserItem script at index {i}.");
            }
        }

        dropDownGroup.InjectAllDropDownShowSelectedItem(toggles, toggleGroup);
        Debug.Log("[DropdownPopulator] Injected toggles into DropDownGroup.");
    }

    public void Clear()
    {
        Debug.Log("[DropdownPopulator] Clearing existing toggles.");

        foreach (Transform child in togglesParent)
        {
            Destroy(child.gameObject);
        }
    }
}
