using TMPro;
using UnityEngine;

public class DifficultyToggle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private UnityEngine.UI.Toggle toggle;

    public void SetText(string text) { displayText.text = text; }
    public void SetToggleGroup(UnityEngine.UI.ToggleGroup toggleGroup) { toggle.group = toggleGroup; }
    public void Select() { toggle.Select(); }
}
