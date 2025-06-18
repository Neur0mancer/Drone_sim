using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    [SerializeField] private GameObject _dronePrefab;

    private Dictionary<Base, List<GameObject>> _dronesPerBase = new Dictionary<Base, List<GameObject>>();

    public void GenerateDrones(Faction faction, Material material, Base homebase, int count)
    {
        ClearDronesForBase(homebase);

        if (!_dronesPerBase.ContainsKey(homebase))
        {
            _dronesPerBase[homebase] = new List<GameObject>();
        }

        for (int i = 0; i < count; i++)
        {
        GameObject newDrone = Instantiate(_dronePrefab, homebase.transform.position, Quaternion.identity, this.transform);
        DroneAI droneAI = newDrone.GetComponent<DroneAI>();
        droneAI.SetFaction(faction);
        droneAI.SetHomebase(homebase);
        newDrone.GetComponentInChildren<Renderer>().material = material;

         _dronesPerBase[homebase].Add(newDrone);
        }
    }
    private void ClearDronesForBase(Base baseKey)
    {
        if (_dronesPerBase.TryGetValue(baseKey, out List<GameObject> drones))
        {
            foreach (GameObject drone in drones)
            {
                if (drone != null)
                    Destroy(drone);
            }
            drones.Clear();
        }
    }
}
