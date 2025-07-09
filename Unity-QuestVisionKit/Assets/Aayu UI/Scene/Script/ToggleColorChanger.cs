using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    [Header("Toggle that changes color on value")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image targetGraphic;

    public Color normalColorOn = new Color(0.5f, 0f, 0.5f); // Purple
    public Color normalColorOff = Color.white;

    [Header("One-time purple toggle (does not change after start)")]
    [SerializeField] private Toggle purpleOnlyToggle;
    [SerializeField] private Image purpleOnlyTargetGraphic;

    private void Start()
    {
        // Regular toggle behavior
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        UpdateColor(toggle.isOn);

        // One-time purple toggle setup
        if (purpleOnlyToggle != null && purpleOnlyTargetGraphic != null)
        {
            ColorBlock purpleColors = purpleOnlyToggle.colors;
            purpleColors.normalColor = normalColorOn;
            purpleOnlyToggle.colors = purpleColors;
            purpleOnlyTargetGraphic.color = normalColorOn;
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        UpdateColor(isOn);
    }

    private void UpdateColor(bool isOn)
    {
        ColorBlock colors = toggle.colors;
        colors.normalColor = isOn ? normalColorOn : normalColorOff;
        toggle.colors = colors;
        targetGraphic.color = colors.normalColor;
    }
}
