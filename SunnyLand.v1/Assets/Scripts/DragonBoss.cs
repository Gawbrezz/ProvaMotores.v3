using UnityEngine;

public class DragonBossVoador : MonoBehaviour
{
    [Header("Movimentação Aérea")]
    public Transform pontoA;
    public Transform pontoB;
    public float velocidade = 4f;

    [Header("Perseguição")]
    public Transform player;
    public float distanciaParaSeguir = 90f;

    [Header("Ataque")]
    public GameObject bolaDeFogoPrefab;
    public float tempoEntreTiros = 3f;
    public float forcaTiro = 8f;

    private float timerTiro = 0f;
    private Vector3 destino;

    private SpriteRenderer sr;

    void Start()
    {
        destino = pontoB.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Mover();
        Atirar();
    }

    // -------------------------------
    //  MOVIMENTO AÉREO REAL
    // -------------------------------
    void Mover()
    {
        float distanciaDoPlayer = Vector2.Distance(transform.position, player.position);

        // Seguir player SE estiver perto
        if (distanciaDoPlayer <= distanciaParaSeguir)
        {
            destino = player.position;
        }
        else
        {
            // Patrulha entre A e B
            if (Vector2.Distance(transform.position, destino) < 0.3f)
            {
                destino = (destino == pontoA.position) ? pontoB.position : pontoA.position;
            }
        }

        // Move voando
        transform.position = Vector2.MoveTowards(
            transform.position,
            destino,
            velocidade * Time.deltaTime
        );

        // Virar sprite
        sr.flipX = destino.x < transform.position.x;
    }

    // -------------------------------
    //  ATAQUE — TIRO TRIPLO REAL
    // -------------------------------
    void Atirar()
    {
        timerTiro -= Time.deltaTime;

        if (timerTiro > 0) return;
        timerTiro = tempoEntreTiros;

        // Direção do tiro
        float direcao = sr.flipX ? -1 : 1;
        Vector2 dir = new Vector2(direcao, 0);

        // Posições laterais para o tiro triplo
        Vector3 cima = new Vector3(0, 0.3f, 0);
        Vector3 meio = Vector3.zero;
        Vector3 baixo = new Vector3(0, -0.3f, 0);

        Disparar(dir, transform.position + cima);
        Disparar(dir, transform.position + meio);
        Disparar(dir, transform.position + baixo);
    }

    void Disparar(Vector2 direcao, Vector3 pos)
    {
        GameObject bola = Instantiate(bolaDeFogoPrefab, pos, Quaternion.identity);
        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();

        rb.linearVelocity = direcao * forcaTiro;
    }
}
