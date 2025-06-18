using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _resourcePrefab;
    [SerializeField] private float _spawnRate = 1;
    private float _spawnRadius = 40f;


    private void Start()
    {
        UI.Instance.generationSpeedInput.onValueChanged.AddListener(OnGenerationSpeedChanged);
        StartCoroutine(SpawnLoop());
    }
    private void GenerateResource()
    {
        Vector3 randomPoint;
        randomPoint = transform.position + Random.insideUnitSphere * _spawnRadius;
        randomPoint.y = 0;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            Instantiate(_resourcePrefab, hit.position, Quaternion.identity, this.transform);            
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            GenerateResource();

            float delay = 60f / Mathf.Max(_spawnRate, 0.01f);
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnGenerationSpeedChanged(string newValue)
    {
        if (int.TryParse(newValue, out int result))
        {
            _spawnRate = result;
        }
        else
        {
            Debug.Log("Need to be a number!");
        }
    }
}
