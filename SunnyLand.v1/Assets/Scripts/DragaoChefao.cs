using UnityEngine;

public class DragaoChefao : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 2f;
    public float alturaVooMin = 2f;
    public float alturaVooMax = 5f;

    [Header("Ataque")]
    public GameObject bolaDeFogoPrefab;
    public float tempoEntreTiros = 2f;
    public float forcaTiro = 7f;

    private float tempoTiroAtual = 0f;
    private Transform player;
    private SpriteRenderer sprite;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player == null) return;

        // -------- Movimento --------
        float direcao = player.position.x > transform.position.x ? 1 : -1;
        sprite.flipX = direcao < 0;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, alturaVooMax, Time.deltaTime * velocidade),
            transform.position.z
        );

        // -------- Ataque --------
        tempoTiroAtual -= Time.deltaTime;
        if (tempoTiroAtual <= 0)
        {
            Atirar3();
            tempoTiroAtual = tempoEntreTiros;
        }
    }

    private void Atirar3()
    {
        if (bolaDeFogoPrefab == null) return;

        float direcao = sprite.flipX ? -1 : 1;

        CriarTiro(0, direcao);      // tiro do meio
        CriarTiro(0.3f, direcao);   // tiro acima
        CriarTiro(-0.3f, direcao);  // tiro abaixo
    }

    private void CriarTiro(float offsetY, float direcao)
    {
        Vector3 pos = transform.position + new Vector3(direcao * 1f, offsetY, 0);

        GameObject tiro = Instantiate(bolaDeFogoPrefab, pos, Quaternion.identity);

        Rigidbody2D rb = tiro.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(forcaTiro * direcao, 0);
        }
    }
}