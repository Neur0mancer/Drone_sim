using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Faction _faction;
    [SerializeField] private GameObject[] _baseParts;
    [SerializeField] private BaseUI _baseUI;
    [SerializeField] private Material[] _materials;
    [SerializeField] private GameObject _effect;

    private int _droneNumber = 5;
    private int _resourceCount;
    private Material _material;
    private DroneControl _droneControl;

    private void Start()
    {
        _material = _materials[(int)_faction];

        foreach (var part in _baseParts)
        {
            part.GetComponent<Renderer>().material = _material;
        }
        _droneControl = FindAnyObjectByType<DroneControl>();

        UI.Instance.droneNumberSlider.onValueChanged.AddListener(OnDroneSliderChanged);
        _droneNumber = (int)UI.Instance.droneNumberSlider.value;

        GenerateDrones();
    }

    public Transform GetCoordinates()
    {
        return this.transform;
    }

    public void AddResource()
    {
        _resourceCount++;
        _baseUI.UpdateResourceText(_resourceCount.ToString());
        PlayeEffect();
    }

    private void OnDroneSliderChanged(float newValue)
    {
        _droneNumber = (int)newValue;
        GenerateDrones();
    }

    private void GenerateDrones()
    {
        _droneControl.GenerateDrones(_faction, _material, this, _droneNumber);
    }

    private void PlayeEffect()
    {
        GameObject effectInstance = Instantiate(_effect, transform.position, Quaternion.identity);
        Destroy(effectInstance, 4f);
    }
}
