using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 10f;
    public float forcaDoPulo = 6f;

    [Header("Rolagem")]
    public float forcaRolagem = 6f;
    public float tempoRolagem = 0.5f;

    [Header("Pulo Extra (Pena)")]
    public bool podePularExtra = false;
    public float forcaPuloExtra = 8f;

    [Header("Wall Slide")]
    public float velocidadeDeslize = -1.5f;
    public LayerMask camadaParede;
    public float distanciaParede = 0.3f;

    [Header("Desequilíbrio")]
    public Vector2 tamanhoCaixa = new Vector2(0.6f, 0.1f);
    public float distanciaCaixa = 0.1f;
    public LayerMask camadaChao;

    [Header("Limbo")]
    public float limiteY = -10f;

    private bool noChao = false;
    private bool andando = false;
    private bool rolando = false;
    private bool desequilibrado = false;
    private bool wallSliding = false;
    private bool usouPuloExtra = false;

    private float tempoRolagemAtual = 0f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ----- Rolagem -----
        if (rolando)
        {
            tempoRolagemAtual -= Time.deltaTime;
            if (tempoRolagemAtual <= 0f) rolando = false;
        }

        andando = false;

        if (!rolando && !desequilibrado && !wallSliding)
        {
            float moveInput = 0f;

            if (Input.GetKey(KeyCode.A)) moveInput = -1f;
            if (Input.GetKey(KeyCode.D)) moveInput = 1f;

            rb.linearVelocity = new Vector2(moveInput * velocidade, rb.linearVelocity.y);

            if (moveInput != 0)
            {
                andando = true;
                sprite.flipX = moveInput < 0;
            }

            // Pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (noChao)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                    rb.AddForce(new Vector2(0, forcaDoPulo), ForceMode2D.Impulse);
                }
                else if (podePularExtra && !usouPuloExtra)
                {
                    PularExtra(forcaPuloExtra);
                    usouPuloExtra = true;
                    podePularExtra = false;
                }
            }

            // Rolagem
            if (Input.GetKeyDown(KeyCode.LeftShift) && noChao)
            {
                rolando = true;
                tempoRolagemAtual = tempoRolagem;
                float direcao = sprite.flipX ? -1 : 1;
                rb.AddForce(new Vector2(direcao * forcaRolagem, 0), ForceMode2D.Impulse);
            }
        }

        // ----- Desequilíbrio e Wall Slide -----
        VerificarDesequilibrio();
        VerificarWallSlide();

        // ----- Animator -----
        animator.SetBool("Andando", andando);
        animator.SetBool("Pulo", !noChao);
        animator.SetBool("Rolando", rolando);
        animator.SetBool("Desequilibrado", desequilibrado);
        animator.SetBool("WallSlide", wallSliding);

        // ----- Limbo -----
        if (transform.position.y < limiteY) Morrer();
    }

    public void PularExtra(float forcaExtra)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(new Vector2(0, forcaExtra), ForceMode2D.Impulse);
    }

    public void AtivarPuloExtra()
    {
        podePularExtra = true;
        usouPuloExtra = false;
    }

    void VerificarDesequilibrio()
    {
        if (noChao)
        {
            Vector2 centro = (Vector2)transform.position + Vector2.down * distanciaCaixa;
            RaycastHit2D hit = Physics2D.BoxCast(centro, tamanhoCaixa, 0f, Vector2.down, 0f, camadaChao);
            if (hit.collider != null)
            {
                float centroChao = hit.collider.bounds.center.x;
                desequilibrado = Mathf.Abs(transform.position.x - centroChao) > (tamanhoCaixa.x * 0.25f);
            }
            else desequilibrado = false;
        }
        else desequilibrado = false;
    }

    void VerificarWallSlide()
    {
        if (!noChao && rb.linearVelocity.y < 0)
        {
            Vector2 direcao = sprite.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaParede, camadaParede);
            if (hit.collider != null)
            {
                wallSliding = true;
                if (rb.linearVelocity.y < velocidadeDeslize)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadeDeslize);
            }
            else wallSliding = false;
        }
        else wallSliding = false;
    }

    public void Morrer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Chao"))
        {
            noChao = true;
            usouPuloExtra = false;
        }
    }

    void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Chao"))
            noChao = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 centro = (Vector2)transform.position + Vector2.down * distanciaCaixa;
        Gizmos.DrawWireCube(centro, tamanhoCaixa);

        Gizmos.color = Color.blue;
        Vector2 direcao = sprite != null && sprite.flipX ? Vector2.left : Vector2.right;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + direcao * distanciaParede);
    }
}
