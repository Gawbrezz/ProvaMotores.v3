using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorObject;   // visual da porta
    public Collider2D doorCollider; // collider que bloqueia o player

    public void OpenDoor()
    {
        if (doorCollider != null)
            doorCollider.enabled = false;

        if (doorObject != null)
            doorObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}