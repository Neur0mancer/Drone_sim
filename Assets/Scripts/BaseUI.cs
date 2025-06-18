using TMPro;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceText;

    public void UpdateResourceText(string text)
    {
        _resourceText.text = text;
    }
}
