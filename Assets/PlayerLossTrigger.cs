using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerLossTrigger : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;   // Speed of smoke movement
    [SerializeField] private GameObject losePanel, SecondaryCamera;   // Assign in Inspector
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Move continuously to the right
        rb.linearVelocity = Vector2.right * moveSpeed;

        // Optional: make collider a trigger so it doesn't physically collide
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to player collisions
        if (other.CompareTag("Player"))
        {
            Debug.Log("Smoke touched player — Game Over!");

            // Disable player
            other.gameObject.SetActive(false);

            // Activate lose panel
            if (losePanel != null)
            {
                losePanel.SetActive(true);
                SecondaryCamera.SetActive(true);
            }
            else
                Debug.LogWarning("Lose panel not assigned in Inspector!");
        }
    }
}
