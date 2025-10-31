using UnityEngine;

public class BolaDeFogo : MonoBehaviour
{
    public float velocidade = 8f;
    public float tempoDeVida = 4f;
    public Vector2 direcao = Vector2.right; // Definir ao instanciar

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.Translate(direcao.normalized * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Usa o método de morte do player, se existir
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.Morrer();

            Destroy(gameObject);
        }

        if (collision.CompareTag("Chao"))
        {
            Destroy(gameObject);
        }
    }
}