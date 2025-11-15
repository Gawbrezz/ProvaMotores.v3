using UnityEngine;

public class Escada : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeSubida = 3f;

    [Header("Debug")]
    public bool estaNaEscada;

    private float gravidadeOriginal;

    private const string PARAM_ESCADA = "Escada";
    private const string PARAM_VELOCIDADE = "VelocidadeEscada";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaNaEscada = true;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Animator animator = other.GetComponent<Animator>();

            gravidadeOriginal = rb.gravityScale; // salva a gravidade verdadeira

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

        if (Mathf.Abs(vertical) > 0.1f)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, vertical * velocidadeSubida);

            if (animator != null)
            {
                animator.SetBool(PARAM_ESCADA, true);
                animator.SetFloat(PARAM_VELOCIDADE, Mathf.Abs(vertical));
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
                animator.SetFloat(PARAM_VELOCIDADE, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Animator animator = other.GetComponent<Animator>();

        estaNaEscada = false;

        rb.gravityScale = gravidadeOriginal; // restaura corretamente

        if (animator != null)
        {
            animator.SetBool(PARAM_ESCADA, false);
            animator.SetFloat(PARAM_VELOCIDADE, 0f);
        }
    }
}
