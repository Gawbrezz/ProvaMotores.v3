using UnityEngine;
using UnityEngine.SceneManagement;

public class Dano : MonoBehaviour
{
    [Header("Configurações de Morte")]
    public float tempoAnimacaoMorte = 0.8f; // tempo em segundos até reiniciar a cena

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // --- Referências ---
            Animator animator = other.GetComponent<Animator>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            // Aqui é importante: substitua "Player" pelo nome EXATO do seu script de movimento!
            // Exemplo: Player, PlayerController, PlayerMovement, etc.
            MonoBehaviour playerScript = other.GetComponent<Player>();

            // --- Ativa animação de dano ---
            if (animator != null)
                animator.SetBool("Dano", true);

            // --- Para o movimento do player ---
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            // --- Desativa o script de controle ---
            if (playerScript != null)
                playerScript.enabled = false;

            // --- Conta a morte imediatamente ---
            FindObjectOfType<DeathCounter>()?.AddDeath();

            // --- Reinicia a cena após a animação ---
            Invoke(nameof(ReiniciarCena), tempoAnimacaoMorte);
        }
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}