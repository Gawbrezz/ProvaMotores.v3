using UnityEngine;

public class DanoInimigo : MonoBehaviour
{
    public float tempoAnimacaoMorte = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = other.GetComponent<Animator>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            MonoBehaviour playerScript = other.GetComponent<Player>();

            if (animator != null)
                animator.SetBool("Dano", true);

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            if (playerScript != null)
                playerScript.enabled = false;

            FindObjectOfType<DeathCounter>()?.AddDeath();

            Invoke(nameof(ReiniciarCena), tempoAnimacaoMorte);
        }
    }

    void ReiniciarCena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}