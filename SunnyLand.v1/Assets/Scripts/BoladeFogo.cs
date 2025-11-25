using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float velocidade = 6f;
    private Vector2 direcao;

    public void SetDirection(Vector2 d)
    {
        direcao = d;
    }

    void Update()
    {
        transform.Translate(direcao * velocidade * Time.deltaTime);

        // opcional: vira a sprite na direção que vai
        if (direcao.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player p = col.GetComponent<Player>();
            if (p != null) p.Morrer();
        }

        Destroy(gameObject);
    }
}
