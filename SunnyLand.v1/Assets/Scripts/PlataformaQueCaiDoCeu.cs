using UnityEngine;

public class FallingPlatformFromSky : MonoBehaviour
{
    [Header("Configurações da Plataforma")]
    public float alturaInicial = 10f;           // Altura inicial da queda
    public float velocidadeQueda = 8f;          // Velocidade da queda
    public float raioDeteccao = 5f;             // Distância para detectar o player
    public LayerMask camadaPlayer;              // Layer do player
    public float tempoParaDestruir = 2f;        // Tempo após cair para destruir

    private bool caiu = false;
    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + alturaInicial, transform.position.z);
    }

    void Update()
    {
        if (!caiu)
        {
            // Detecta se o player está próximo
            Collider2D player = Physics2D.OverlapCircle(posicaoInicial, raioDeteccao, camadaPlayer);
            if (player != null)
            {
                caiu = true;
            }
        }
        else
        {
            // Move a plataforma para baixo
            transform.position += Vector3.down * velocidadeQueda * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = other.GetComponent<Animator>();
            Rigidbody2D rbPlayer = other.GetComponent<Rigidbody2D>();
            Player playerScript = other.GetComponent<Player>();

            if (animator != null)
                animator.SetBool("Dano", true); // Ativa a animação de dano

            if (rbPlayer != null)
            {
                rbPlayer.linearVelocity = Vector2.zero;                       // Zera o movimento
                rbPlayer.gravityScale = 0;                                    // Desativa a gravidade
                rbPlayer.constraints = RigidbodyConstraints2D.FreezeAll;       // Congela o corpo
            }

            if (playerScript != null)
            {
                // Conta a morte e reinicia após um pequeno atraso
                playerScript.Invoke("Morrer", 0.5f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoInicial == Vector3.zero ? transform.position : posicaoInicial, raioDeteccao);
    }
}
