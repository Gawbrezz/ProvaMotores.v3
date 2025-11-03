using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public Door doorToOpen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Key: OnTriggerEnter2D with " + other.name); // temporário para debugar
        if (other.CompareTag("Player"))
        {
            if (doorToOpen != null)
                doorToOpen.OpenDoor();
            Destroy(gameObject);
        }
    }
}