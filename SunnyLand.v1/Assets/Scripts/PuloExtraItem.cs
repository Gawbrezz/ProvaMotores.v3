using UnityEngine;

public class ItemPuloExtra : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AtivarPuloExtra(); // habilita o pulo extra
                Destroy(gameObject); // remove o item da cena
            }
        }
    }
}