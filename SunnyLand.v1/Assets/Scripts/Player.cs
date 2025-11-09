using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movimento")] public float velocidade = 10f;
    public float forcaDoPulo = 6f;

    [Header("Rolagem")] public float forcaRolagem = 6f;
    public float tempoRolagem = 0.5f;

    [Header("Pulo Extra (Pena)")] public bool podePularExtra = false;
    public float forcaPuloExtra = 8f;

    [Header("Limbo")] public float limiteY = -10f;

    private bool noChao = false;
    private bool andando = false;
    private bool rolando = false;
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

        if (!rolando)
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

        // ----- Animator -----
        animator.SetBool("Andando", andando);
        animator.SetBool("Pulo", !noChao);
        animator.SetBool("Rolando", rolando);

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

    public void Morrer()
    {
        // ✅ Conta uma morte
        FindObjectOfType<DeathCounter>()?.AddDeath();

        // ✅ Recarrega a cena após contar a morte
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
}