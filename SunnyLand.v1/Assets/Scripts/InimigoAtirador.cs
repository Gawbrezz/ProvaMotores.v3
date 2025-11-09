using UnityEngine;

public class InimigoAtirador : MonoBehaviour
{
    public GameObject projetilPrefab;   // Prefab da bola de fogo
    public Transform pontoDisparo;      // Onde o tiro sai (ex: mão ou boca do inimigo)
    public float forcaTiro = 10f;       // Velocidade da bola de fogo
    public float tempoEntreTiros = 2f;  // Tempo entre cada disparo
    public float distanciaDeAtaque = 10f; // Distância para começar a atirar

    private Transform player;
    private float tempoProximoTiro;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tempoProximoTiro = Time.time;
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        // Só atira se o player estiver perto o suficiente
        if (distancia <= distanciaDeAtaque && Time.time >= tempoProximoTiro)
        {
            Atirar();
            tempoProximoTiro = Time.time + tempoEntreTiros;
        }
    }

    void Atirar()
    {
        // Cria o projetil
        GameObject fogo = Instantiate(projetilPrefab, pontoDisparo.position, pontoDisparo.rotation);

        // Calcula a direção para o player
        Vector2 direcao = (player.position - pontoDisparo.position).normalized;

        // Adiciona força na direção
        Rigidbody2D rb = fogo.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direcao * forcaTiro;

        // Destroi o projetil após alguns segundos
        Destroy(fogo, 5f);
    }
}