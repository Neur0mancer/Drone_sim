using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] public Slider droneNumberSlider;
    [SerializeField] public Slider droneSpeedSlider;
    [SerializeField] public TMP_InputField generationSpeedInput;
    [SerializeField] public Toggle visualizePathToggle;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    public float GetGenerationSpeed()
    {
        float result = 1f;
        float.TryParse(generationSpeedInput.text, out result);
        return result;
    }

    public bool IsPathVisualizationEnabled()
    {
        return visualizePathToggle.isOn;
    }

    public void DrawTarget(NavMeshPath path)
    {
        if (path.corners.Length < 2) return;
        _lineRenderer.positionCount = path.corners.Length;
        _lineRenderer.SetPositions(path.corners);
    }
}

