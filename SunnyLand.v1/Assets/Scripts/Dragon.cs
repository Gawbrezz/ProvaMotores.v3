using UnityEngine;

public class BossDragon : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public Transform player;
    public float distanciaRajada = 3f;
    public float fireballCooldown = 2f;
    public float breathCooldown = 3f;

    [Header("Prefabs de Ataques")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public GameObject breathPrefab;

    [Header("Pulo")]
    public float jumpForce = 8f;
    public float jumpIntervalMin = 3f;
    public float jumpIntervalMax = 6f;

    private Rigidbody2D rb;
    private Animator anim;

    private float proximoFireball;
    private float proximoBreath;
    private float proximoPulo;

    private bool podeVoar = false; // futuro upgrade

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        proximoPulo = Time.time + Random.Range(jumpIntervalMin, jumpIntervalMax);
    }

    void Update()
    {
        float distancia = Vector2.Distance(transform.position, player.position);

        // ========== ATAQUE RAJADA (perto) ==========
        if (distancia <= distanciaRajada && Time.time >= proximoBreath)
        {
            Rajada();
        }

        // ========== ATAQUE FIREBALL (longe) ==========
        else if (Time.time >= proximoFireball)
        {
            Fireball();
        }

        // ========== PULO ==========
        if (Time.time >= proximoPulo)
        {
            Pular();
            proximoPulo = Time.time + Random.Range(jumpIntervalMin, jumpIntervalMax);
        }
    }

    void Fireball()
{
    anim.SetTrigger("Fireball");

    // cria a bola
    GameObject bola = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

    // calcula direção até o player
    Vector2 direcao = (player.position - firePoint.position).normalized;

    // envia a direção para o script da bala
    bola.GetComponent<Fireball>().SetDirection(direcao);

    proximoFireball = Time.time + fireballCooldown;
}


    void Rajada()
    {
        anim.SetTrigger("Breath");
        GameObject fumaça = Instantiate(breathPrefab, firePoint.position, firePoint.rotation);
        Destroy(fumaça, 1.5f);
        proximoBreath = Time.time + breathCooldown;
    }

    void Pular()
    {
        if (podeVoar) return; // caso no futuro ele passe a voar
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }
}
