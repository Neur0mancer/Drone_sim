using TMPro;
using UnityEngine;

public class DroneStateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statusText;


    public void SetStatusText(string text)
    {
        _statusText.text = text;
    }
}
