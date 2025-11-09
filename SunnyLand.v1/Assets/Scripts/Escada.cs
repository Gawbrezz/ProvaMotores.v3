using UnityEngine;

public class Escada : MonoBehaviour
{
    public float velocidadeSubida = 3f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Animator animator = other.GetComponent<Animator>();
            if (rb == null || animator == null) return;

            float vertical = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(vertical) > 0.1f)
            {
                // Ativa a animação de escalar
                animator.SetBool("Escada", true);

                // Desativa gravidade enquanto sobe ou desce
                rb.gravityScale = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * velocidadeSubida);
            }
            else
            {
                // Para o jogador quando solta o botão
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

                // Mantém a animação de escada se ele ainda estiver na escada, mas parado
                animator.SetBool("Escada", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Animator animator = other.GetComponent<Animator>();
            if (rb == null || animator == null) return;

            // Restaura gravidade e desativa animação
            rb.gravityScale = 3f; // ajuste conforme a gravidade padrão do player
            animator.SetBool("Escada", false);
        }
    }
}