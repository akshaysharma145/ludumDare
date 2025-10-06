using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorMixtureStation : MonoBehaviour
{
    public GameObject Sprit1, Sprit2;
    public PlayerInventory playerInventory; // Reference to player's inventory (set when player enters)

    [HideInInspector] public bool playerNearby = false;

    public void Mixture()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory is not assigned!");
            return;
        }

        Debug.Log("Mixing colors at station");

        playerNearby = true;
        Sprit2.SetActive(true);
        Sprit1.SetActive(false);
    }

    public void Close()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory is not assigned!");
            return;
        }

        Debug.Log("Closing mixture station");

        playerNearby = false;
        Sprit1.SetActive(true);
        Sprit2.SetActive(false);

        playerInventory.Deselect();
    }
}
