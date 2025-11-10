using UnityEngine;

public class Escada : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeSubida = 3f;
    public float gravidadePadrao = 3f;

    [Header("Debug")]
    public bool estaNaEscada;

    private const string PARAM_ESCADA = "Escada";
    private const string PARAM_VELOCIDADE = "VelocidadeEscada";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaNaEscada = true;
            Animator animator = other.GetComponent<Animator>();
            if (animator != null)
                animator.SetBool(PARAM_ESCADA, true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Animator animator = other.GetComponent<Animator>();
        if (rb == null) return;

        float vertical = Input.GetAxisRaw("Vertical");

        // 🔹 Se o jogador estiver subindo ou descendo
        if (Mathf.Abs(vertical) > 0.1f)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, vertical * velocidadeSubida);

            // Atualiza a velocidade da animação
            if (animator != null)
            {
                animator.SetBool(PARAM_ESCADA, true);
                animator.SetFloat(PARAM_VELOCIDADE, Mathf.Abs(vertical)); // controla a velocidade do loop
            }
        }
        else
        {
            // parado na escada, animação continua (mas movimento pausa)
            rb.linearVelocity = new Vector2(0f, 0f);

            if (animator != null)
                animator.SetFloat(PARAM_VELOCIDADE, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Animator animator = other.GetComponent<Animator>();
        if (rb == null) return;

        estaNaEscada = false;

        // 🔹 restaura tudo ao normal
        rb.gravityScale = gravidadePadrao;
        if (animator != null)
        {
            animator.SetBool(PARAM_ESCADA, false);
            animator.SetFloat(PARAM_VELOCIDADE, 0f);
        }
    }
}
