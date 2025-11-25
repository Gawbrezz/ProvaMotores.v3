using UnityEngine;

public class DragonVoador : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 4f;
    public float alturaMin = 2f;
    public float alturaMax = 5f;
    public float velocidadeVertical = 2f;

    [Header("Player")]
    public Transform player;

    [Header("Ataque Fireball")]
    public GameObject fireball;
    public Transform firePoint;
    public float intervaloTiro = 3f;

    private float contadorTiro = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private float alturaAlvo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // primeira altura aleatória
        alturaAlvo = Random.Range(alturaMin, alturaMax);
    }

    private void Update()
    {
        if (player == null) return;

        // --- FLIP CORRIGIDO ---
        sprite.flipX = player.position.x > transform.position.x;


        // --- MOVIMENTO HORIZONTAL ---
       float direcao = sprite.flipX ? 1 : -1;

        rb.linearVelocity = new Vector2(direcao * velocidade, rb.linearVelocity.y);

        // --- MOVIMENTO VERTICAL SUAVE ---
        if (Mathf.Abs(transform.position.y - alturaAlvo) < 0.5f)
            alturaAlvo = Random.Range(alturaMin, alturaMax);

        float novaY = Mathf.MoveTowards(
            transform.position.y,
            alturaAlvo,
            Time.deltaTime * velocidadeVertical
        );

        transform.position = new Vector3(transform.position.x, novaY, transform.position.z);

        // --- ATAQUE ---
        contadorTiro -= Time.deltaTime;
        if (contadorTiro <= 0)
        {
            AtirarSpread();
            contadorTiro = intervaloTiro;
        }
    }

    private void AtirarSpread()
    {
        if (fireball == null || firePoint == null) return;

       float direcao = sprite.flipX ? 1 : -1;


        CriarTiro( 0.8f, 6f, direcao); // fireball de cima
        CriarTiro( 0f,   7f, direcao); // fireball do meio
        CriarTiro(-0.8f, 6f, direcao); // fireball de baixo
    }

    private void CriarTiro(float offsetY, float speed, float direcao)
    {
        Vector3 pos = firePoint.position + new Vector3(0, offsetY, 0);
        GameObject f = Instantiate(fireball, pos, Quaternion.identity);

        Rigidbody2D rbF = f.GetComponent<Rigidbody2D>();
        rbF.linearVelocity = new Vector2(speed * direcao, 0);
    }
}
