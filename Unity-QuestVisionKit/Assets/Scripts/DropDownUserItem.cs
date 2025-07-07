using TMPro;
using UnityEngine;

public class DropdownUserItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userLabel;

    public void SetUsername(string username)
    {
        if (userLabel != null)
        {
            userLabel.text = username;
        }
    }
}
