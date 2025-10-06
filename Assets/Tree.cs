using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Tree : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text floatingTexttree;         // Text to show when near
    public Button paintButton;          // Mix button
    public Animator animator;
    public PlayerInventory playerinventory; // Reference to player's inventory (set when player enters)
    public float animationTime;
    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    [HideInInspector] public bool playernearby = false;

    void Start()
    {
        if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(false);
        if (paintButton != null)
        {
            paintButton.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playernearby = true;
        if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(true);
        if (paintButton != null) paintButton.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playernearby = false;
        if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(false);
        if (paintButton != null) paintButton.gameObject.SetActive(false);
    }
    public void PaintTree()
    {
        if (playerinventory.bag.Contains("Green"))
        {
            if (animator != null)
            {
                animator.SetTrigger("TreePaint");
                //animator2.SetTrigger("PlayerAni1");
                playerinventory.RemoveGreen();
                playerMovement.TreeOverride();
            }
            Debug.Log("Tree painted!");
        }
        // Add your painting logic here
    }

}