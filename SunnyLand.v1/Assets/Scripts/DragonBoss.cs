using UnityEngine;

public class DragonBossVoador : MonoBehaviour
{
    [Header("Movimentação")]
    public Transform[] pontosPatrulha;
    private int indexAtual = 0;
    public float velocidade = 3f;
    public float distanciaPlayerParaPerseguir = 8f;

    [Header("Tiro")]
    public GameObject fireballPrefab;
    public float intervaloTiro = 2f;
    private float tempoTiro = 0f;

    [Header("Referências")]
    public Transform player;
    public Transform pontoTiroCentro;
    public Transform pontoTiroEsquerda;
    public Transform pontoTiroDireita;

    void Update()
    {
        if (player == null) return;

        // ---- FLIP CORRIGIDO ----
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);   // olhando pra direita
        else
            transform.localScale = new Vector3(-1, 1, 1);  // olhando pra esquerda

        float distancia = Vector2.Distance(transform.position, player.position);

        // Se o player estiver perto → seguir ele
        if (distancia <= distanciaPlayerParaPerseguir)
            SeguirPlayer();
        else
            Patrulhar();

        // Atirar
        tempoTiro += Time.deltaTime;
        if (tempoTiro >= intervaloTiro)
        {
            AtirarTres();
            tempoTiro = 0f;
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0) return;

        Transform alvo = pontosPatrulha[indexAtual];
        transform.position = Vector2.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime);

        if (Vector2.Distance(transform.position, alvo.position) < 0.3f)
            indexAtual = (indexAtual + 1) % pontosPatrulha.Length;
    }

    void SeguirPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, velocidade * Time.deltaTime);
    }

    void AtirarTres()
    {
        // Direção correta baseada no flip
        int direcao = transform.localScale.x > 0 ? 1 : -1;

        AtirarUma(pontoTiroCentro, direcao);
        AtirarUma(pontoTiroEsquerda, direcao);
        AtirarUma(pontoTiroDireita, direcao);
    }

    void AtirarUma(Transform ponto, int direcao)
    {
        GameObject fireball = Instantiate(fireballPrefab, ponto.position, Quaternion.identity);

        // Faz a bola de fogo ir sempre pra frente do dragão
        fireball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direcao * 5f, 0);
    }
}
