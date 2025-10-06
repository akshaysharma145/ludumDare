using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canMove = true;   // Movement lock flag
    public Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;  // block motion during override

        // Works with both input systems
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // stop motion when locked
        }
    }

    /// <summary>
    /// Smoothly moves the player to a target Transform within 1 second.
    /// Player movement is disabled during this motion.
    /// </summary>
    public void TreeOverride()
    {
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        if (target == null)
        {
            Debug.LogWarning("TreeOverride: Target is null!");
            yield break;
        }

        canMove = false; // disable player control

        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos; // ensure final position exact
        canMove = true; // re-enable player control
    }
}
