using UnityEngine;

public class SmoothCameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Smoothness")]
    public float smoothTime = 0.2f; // Lower = snappier, higher = smoother
    private Vector3 velocity = Vector3.zero;

    [Header("Y-Axis Behavior")]
    public float baseY = 0f;        // Default camera Y
    public float yThreshold = 2f;   // Start raising camera after this
    public float maxYOffset = 3f;   // Max extra Y above base

    [Header("Horizontal Boundaries")]
    public float leftLimit = -10f;
    public float rightLimit = 10f;

    void FixedUpdate() // <-- Use FixedUpdate for physics-based player
    {
        if (!player) return;

        // --- Target X ---
        float targetX = Mathf.Clamp(player.position.x, leftLimit, rightLimit);

        // --- Target Y ---
        float targetY = baseY;
        if (player.position.y > baseY + yThreshold)
        {
            float extraY = Mathf.Min(player.position.y - (baseY + yThreshold), maxYOffset);
            targetY += extraY;
        }

        Vector3 targetPos = new Vector3(targetX, targetY, transform.position.z);

        // --- SmoothDamp for smooth lag (better than Lerp for jitter) ---
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftLimit, -100f, 0f), new Vector3(leftLimit, 100f, 0f));
        Gizmos.DrawLine(new Vector3(rightLimit, -100f, 0f), new Vector3(rightLimit, 100f, 0f));
    }
}
