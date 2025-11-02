using UnityEngine;

public class Dano : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Morrer(); // chama o método do player que conta a morte + reseta cena
            }
        }
    }
}