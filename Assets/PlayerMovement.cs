using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float highJumpForce = 11f;

    [Header("Jump Settings")]
    public float doubleTapTime = 0.6f; // max delay between taps for high jump

    [Header("References")]
    public Transform target;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool canMove = true;
    private bool isGrounded = false;
    private float lastJumpTime = 0f;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;

        // Movement Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.Normalize();

        // --- Handle Facing Direction ---
        if (moveInput.x > 0 && !facingRight)
            Flip(true);
        else if (moveInput.x < 0 && facingRight)
            Flip(false);

        // --- Update Walking Animation ---
        if (animator != null)
            animator.SetBool("walking", Mathf.Abs(moveInput.x) > 0.01f);

        // --- Jump Logic ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                float timeSinceLastJump = Time.time - lastJumpTime;

                if (timeSinceLastJump <= doubleTapTime)
                    HighJump();
                else
                    Jump();

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
        isGrounded = false;

        if (animator != null)
            animator.SetTrigger("JumpTrigger");

        Debug.Log("Normal Jump");
    }

    private void HighJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, highJumpForce);
        isGrounded = false;

        if (animator != null)
            animator.SetTrigger("JumpTrigger");

        Debug.Log("High Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Basic ground check
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1 : -1);
        transform.localScale = scale;
    }

    // Smooth move override function
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
