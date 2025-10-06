using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Target (Player) to follow")]
    public Transform player;

    [Tooltip("How smoothly camera follows the player")]
    public float smoothSpeed = 5f;

    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("CameraFollow: Player not assigned!");
            return;
        }

        // Keep the initial Z distance
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Target position (follow only X and Y, keep Z fixed)
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
