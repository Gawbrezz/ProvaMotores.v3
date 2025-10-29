using UnityEngine;

public class InimigoAndador : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;

    [Header("Configurações do Inimigo")]
    public float distanciaDeVisao = 10f;
    public float velocidade = 3f;
    public float tempoEntreMovimentos = 0.2f; // tempo de resposta

    private float tempoAtual;
    private bool perseguindo = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.transform.position);
        perseguindo = distancia <= distanciaDeVisao;

        if (perseguindo)
        {
            tempoAtual -= Time.deltaTime;

            if (tempoAtual <= 0)
            {
                MoverAtePlayer();
                tempoAtual = tempoEntreMovimentos;
            }

            animator.SetBool("correndo", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("correndo", false);
        }
    }

    void MoverAtePlayer()
    {
        if (player == null) return;

        float direcao = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direcao * velocidade, rb.linearVelocity.y);

        // Inverte o sprite conforme direção
        spriteRenderer.flipX = direcao > 0;
    }

    void OnDrawGizmosSelected()
    {
        // Exibe o raio de visão no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeVisao);
    }
}