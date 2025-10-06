using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlatformManager : MonoBehaviour
{
    private List<Vector3> platformPositions;
    private List<GameObject> platformList;

    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject platformsUI;

    private void Awake() {
        platformPositions = new List<Vector3>();
        platformsUI.SetActive(true);
    }

    public void AddPlatform(Vector3 platformLocation) {
        platformPositions.Add(platformLocation);
    }

    public void RemovePlatform(Vector3 platformLocation) {
        platformPositions.Remove(platformLocation);
    }

    public void PlacePlatforms() {
        foreach (Vector3 pos in platformPositions) {
            Instantiate(platformPrefab, pos, Quaternion.identity);
        }
    }

    public void BuildLevel() {
        PlacePlatforms();
        platformsUI.SetActive(false);

    }
}
