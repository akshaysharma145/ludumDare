using UnityEngine;

public class ActiveOnTrigger : MonoBehaviour
{
    [Tooltip("Object to activate/deactivate when player enters/exits.")]
    public GameObject objectX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && objectX != null)
        {
            objectX.SetActive(true);
            Debug.Log("Player entered trigger → Object activated");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && objectX != null)
        {
            objectX.SetActive(false);
            Debug.Log("Player exited trigger → Object deactivated");
        }
    }
}
