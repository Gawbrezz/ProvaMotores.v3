using UnityEngine;

public class AtivarLavaTrigger : MonoBehaviour
{
    public LavaSubir lava;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lava.AtivarLava();
        }
    }
}