using UnityEngine;

public class LavaSubir : MonoBehaviour
{
    public float velocidade = 2f;
    public Transform limiteSuperior;

    private bool ativada = false;

    void Update()
    {
        if (ativada && transform.position.y < limiteSuperior.position.y)
        {
            transform.Translate(Vector2.up * velocidade * Time.deltaTime);

            if (transform.position.y > limiteSuperior.position.y)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    limiteSuperior.position.y,
                    transform.position.z
                );
            }
        }
    }

    public void AtivarLava()
    {
        ativada = true;
    }
}