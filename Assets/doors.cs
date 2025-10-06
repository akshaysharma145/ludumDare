using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class doors : MonoBehaviour
{
    //[Header("UI Elements")]
    //public TMP_Text floatingTexttree;         // Text to show when near
    public Button doorButton;          // Mix button
    public Animator animatoR;

    [HideInInspector] public bool playernearby = false;

    void Start()
    {
        //if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(false);
        if (doorButton != null)
        {
            doorButton.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playernearby = true;
        //if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(true);
        if (doorButton != null) doorButton.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playernearby = false;
        //if (floatingTexttree != null) floatingTexttree.gameObject.SetActive(false);
        if (doorButton != null) doorButton.gameObject.SetActive(false);
    }
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer sprite3;

    // Reference to your Bag (update to match your bag script name)
    public PlayerInventory colorBag;

    public void ApplyColors()
    {
        // Get available colors from the bag
        bool hasRed = colorBag.bag.Contains("Red") || sprite1.color == Color.red;
        bool hasGreen = colorBag.bag.Contains("Green") || sprite2.color == Color.green;
        bool hasBlue = colorBag.bag.Contains("Blue") || sprite3.color == Color.blue;

        if (hasRed && sprite1.color != Color.red)
        {
            sprite1.color = Color.red;
            colorBag.RemoveRed();
        }
        if (hasGreen && sprite2.color != Color.green)
        {
            sprite2.color = Color.green;
            colorBag.RemoveGreen();
        }
        if (hasBlue && sprite3.color != Color.blue)
        {
            sprite3.color = Color.blue;
            colorBag.RemoveBlue();
        }
        if (hasRed && hasGreen && hasBlue)
        {
            Debug.Log("Open");
            if (animatoR != null)
            {
                animatoR.SetTrigger("OpenDoor");
            }
            StartCoroutine(WaitAndPrint(1f)); // Wait for 2 seconds before loading next scene
        }
    }
    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Waited for " + waitTime + " seconds");
    }
}