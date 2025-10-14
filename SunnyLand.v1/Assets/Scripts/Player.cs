using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float velocidade = 40;
    public float forcaDoPulo = 4;
    public float forcaRolagem = 6f;
    public float tempoRolagem = 0.5f;

    [Header("Pulo Extra (Pena)")]
    public bool podePularExtra = false; // se o jogador pode usar o pulo extra
    public float forcaPuloExtra = 20f;   // força configurável no Inspector

    [Header("Wall Slide")]
    public float velocidadeDeslize = -1.5f;
    public LayerMask camadaParede;
    public float distanciaParede = 0.3f;

    private bool noChao = false;
    private bool andando = false;
    private bool rolando = false;
    private bool desequilibrado = false;
    private bool wallSliding = false;
    private bool usouPuloExtra = false; // controle interno

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private float tempoRolagemAtual = 0f;

    [Header("Desequilíbrio (BoxCast)")]
    public Vector2 tamanhoCaixa = new Vector2(0.6f, 0.1f);
    public float distanciaCaixa = 0.1f;
    public LayerMask camadaChao;

    [Header("Limbo")]
    public float limiteY = -10f;

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
            if (tempoRolagemAtual <= 0f)
                rolando = false;
        }

        andando = false;

        if (!rolando && !desequilibrado && !wallSliding)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-velocidade * Time.deltaTime, 0, 0);
                sprite.flipX = true;
                andando = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);
                sprite.flipX = false;
                andando = true;
            }

            // Pulo normal
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (noChao)
                {
                    rb.AddForce(new Vector2(0, forcaDoPulo), ForceMode2D.Impulse);
                }
                else if (podePularExtra && !usouPuloExtra)
                {
                    PularExtra(forcaPuloExtra);
                    usouPuloExtra = true; // gasta o pulo extra
                    podePularExtra = false; // desativa a habilidade
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && noChao)
            {
                rolando = true;
                tempoRolagemAtual = tempoRolagem;
                float direcao = sprite.flipX ? -1 : 1;
                rb.AddForce(new Vector2(direcao * forcaRolagem, 0), ForceMode2D.Impulse);
            }
        }

        // ----- Checar Desequilíbrio -----
        VerificarDesequilibrio();

        // ----- Checar Wall Slide -----
        VerificarWallSlide();

        // ----- Animator -----
        animator.SetBool("Andando", andando);
        animator.SetBool("Pulo", !noChao);
        animator.SetBool("Rolando", rolando);
        animator.SetBool("Desequilibrado", desequilibrado);
        animator.SetBool("WallSlide", wallSliding);

        // ----- Checar Limbo -----
        if (transform.position.y < limiteY)
            Morrer();
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
                float centroChao = hit.point.x;
                desequilibrado = Mathf.Abs(transform.position.x - centroChao) > (tamanhoCaixa.x * 0.25f);
            }
            else
                desequilibrado = false;
        }
        else
            desequilibrado = false;
    }

    void VerificarWallSlide()
    {
        if (!noChao && rb.linearVelocity.y < 0)
        {
            Vector2 direcao = sprite.flipX ? Vector2.left : Vector2.right;
            Vector2 origem = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(origem, direcao, distanciaParede, camadaParede);

            if (hit.collider != null)
            {
                wallSliding = true;
                if (rb.linearVelocity.y < velocidadeDeslize)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadeDeslize);
            }
            else
                wallSliding = false;
        }
        else
            wallSliding = false;
    }

    void Morrer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Chao"))
        {
            noChao = true;
            usouPuloExtra = false; // reseta ao tocar o chão
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
