using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorMixtureStation : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text floatingText;         // Text to show when near
    public Button mixButton;          // Mix button
    public Transform spawnPoint;      // Where new color box will appear (optional)
    public GameObject Sprite1,Sprite2;
    public PlayerInventory playerInventory; // Reference to player's inventory (set when player enters)

    [HideInInspector] public bool playerNearby = false;

    void Start()
    {
        if (floatingText != null) floatingText.gameObject.SetActive(false);
        Sprite1.SetActive(false);
        Sprite2.SetActive(false);
        if (mixButton != null)
        {
            mixButton.gameObject.SetActive(false);
            //mixButton.onClick.AddListener(MixColors);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = true;
        if (floatingText != null) floatingText.gameObject.SetActive(true);
        Sprite1.SetActive(true);
        Sprite2.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;
        if (floatingText != null) floatingText.gameObject.SetActive(false);
        if (mixButton != null) mixButton.gameObject.SetActive(false);
        Sprite1.SetActive(false);
        Sprite2.SetActive(false);
        playerInventory.Deselect();
    }

}