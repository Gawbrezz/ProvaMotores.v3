using UnityEngine;

public class PlataformaMovelVertical : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Configurações da Plataforma")]
    public float velocidade = 2f;            // Velocidade de movimento
    public float distancia = 3f;             // Distância total do movimento (metade pra cima e metade pra baixo)
    public bool iniciaSubindo = true;        // Direção inicial do movimento

    private Vector2 posicaoInicial;
    private bool subindo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = transform.position;
        subindo = iniciaSubindo;
    }

    void FixedUpdate()
    {
        MoverPlataforma();
    }

    void MoverPlataforma()
    {
        // Move para cima ou para baixo dependendo do estado
        if (subindo)
        {
            rb.linearVelocity = new Vector2(0, velocidade);

            // Se passou do ponto máximo, inverte direção
            if (transform.position.y >= posicaoInicial.y + distancia)
            {
                subindo = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, -velocidade);

            // Se passou do ponto mínimo, inverte direção
            if (transform.position.y <= posicaoInicial.y - distancia)
            {
                subindo = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Mostra no editor o limite de movimento
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - distancia),
                        new Vector2(transform.position.x, transform.position.y + distancia));
    }
}
