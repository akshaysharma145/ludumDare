using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float highJumpForce = 11f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canMove = true;
    public Transform target;

    // Jump handling
    private bool isGrounded = false;
    private float lastJumpTime = 0f;
    public float doubleTapTime = 0.6f; // max delay between taps for high jump

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        // Movement Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                float timeSinceLastJump = Time.time - lastJumpTime;

                if (timeSinceLastJump <= doubleTapTime)
                {
                    HighJump();
                }
                else
                {
                    Jump();
                }

                lastJumpTime = Time.time;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        Debug.Log("Normal Jump");
        isGrounded = false;
    }

    private void HighJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, highJumpForce);
        Debug.Log("High Jump");
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Basic ground check
        if (collision.contacts.Length > 0)
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                isGrounded = true;
            }
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

        canMove = false;

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

        transform.position = endPos;
        canMove = true;
    }
}
