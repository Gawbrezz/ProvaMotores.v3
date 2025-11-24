using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 3f;
    public float alturaPulo = 4f;
    public float tempoEntrePulos = 2f;

    [Header("Ataque")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float tempoEntreTiros = 3f;

    [Header("Breath")]
    public GameObject breathPrefab;
    public float tempoEntreBreaths = 6f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private float contadorPulo;
    private float contadorTiro;
    private float contadorBreath;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        contadorPulo = tempoEntrePulos;
        contadorTiro = tempoEntreTiros;
        contadorBreath = tempoEntreBreaths;
    }

    private void Update()
    {
        // --- Movimento simples ---
        rb.linearVelocity = new Vector2(velocidade, rb.linearVelocity.y);

        // Inverte direção ao bater em algo
        sprite.flipX = velocidade < 0;

        // --- PULO ---
        contadorPulo -= Time.deltaTime;
        if (contadorPulo <= 0)
        {
            rb.AddForce(Vector2.up * alturaPulo, ForceMode2D.Impulse);
            contadorPulo = tempoEntrePulos;
        }

        // --- TIRO (1 bola de fogo) ---
        contadorTiro -= Time.deltaTime;
        if (contadorTiro <= 0)
        {
            Atirar();
            contadorTiro = tempoEntreTiros;
        }

        // --- BREATH ---
        contadorBreath -= Time.deltaTime;
        if (contadorBreath <= 0)
        {
            SoltarBreath();
            contadorBreath = tempoEntreBreaths;
        }
    }

    private void Atirar()
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject bola = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        float direcao = sprite.flipX ? -1 : 1;

        Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
        rbBola.linearVelocity = new Vector2(6f * direcao, 0);
    }

    private void SoltarBreath()
    {
        if (breathPrefab == null) return;

        Vector3 pos = transform.position + new Vector3(sprite.flipX ? -1.2f : 1.2f, 0, 0);
        GameObject b = Instantiate(breathPrefab, pos, Quaternion.identity);

        // vira o breath junto do dragão
        if (sprite.flipX)
            b.transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Inverte direção ao tocar parede ou borda
        if (col.collider.CompareTag("Chao") == false)
        {
            velocidade *= -1;
        }
    }
}
