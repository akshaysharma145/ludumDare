using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSelector : MonoBehaviour
{
    [SerializeField] private GameObject buttonOn;
    [SerializeField] private GameObject buttonOff; 

    [SerializeField] GameObject selectedPlatformManagerGO;
    SelectedPlatformManager selectedPlatformManager;
    Vector3 platformWorldLocation;

    private void Awake() {
        selectedPlatformManager = selectedPlatformManagerGO.GetComponent<SelectedPlatformManager>();
    }

    public void ActivatePlatform() {
        buttonOn.SetActive(false);
        buttonOff.SetActive(true);

        Vector3 screenPos = buttonOn.transform.position; 
        screenPos.z = 10f; 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        selectedPlatformManager.AddPlatform(worldPos); 
    }
    public void DeactivatePlatform() { 
        buttonOff.SetActive(false);
        buttonOn.SetActive(true);

        Vector3 screenPos = buttonOff.transform.position;
        screenPos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        selectedPlatformManager.RemovePlatform(worldPos);
    }

}
