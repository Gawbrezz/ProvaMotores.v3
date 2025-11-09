using UnityEngine;

public class PlataformaQueCai : MonoBehaviour
{
    [Header("Configurações da Plataforma")]
    public float tempoAntesDeCair = 0.5f;  // tempo em segundos antes da plataforma cair
    public float tempoParaReaparecer = 3f; // tempo para reaparecer (coloque 0 se não quiser que volte)

    private Rigidbody2D rb;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;

        rb.bodyType = RigidbodyType2D.Kinematic; // começa parada
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(FazerCair), tempoAntesDeCair);
        }
    }

    void FazerCair()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // plataforma cai
        if (tempoParaReaparecer > 0)
        {
            Invoke(nameof(ReiniciarPlataforma), tempoParaReaparecer);
        }
        else
        {
            Destroy(gameObject, 5f); // se não quiser reaparecer, destrói depois de um tempo
        }
    }

    void ReiniciarPlataforma()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.position = posicaoInicial;
        transform.rotation = rotacaoInicial;
    }
}
