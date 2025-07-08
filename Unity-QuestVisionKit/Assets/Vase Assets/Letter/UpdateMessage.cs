using TMPro;
using UnityEngine;

public class UpdateMessage : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string newText = "Congratulations!";
    
    void Start()
    {
        ChangeText();
    }
    
    public void ChangeText()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = newText;
        }
    }
    
    public void ChangeText(string customText)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = customText;
        }
    }
}
