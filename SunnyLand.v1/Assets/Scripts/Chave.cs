using UnityEngine;

public class Chave : MonoBehaviour
{
    // Opcional: arraste aqui um GameObject (ex: ícone de inventário, texto "Pegou!", etc.)
    public GameObject Colect;

    // Tag do player (padrão "Player")
    public string playerTag = "Player";

    private void Start()
    {
        // só um exemplo: garantir que Colect comece desativado (se quiser)
        if (Colect != null) Colect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Ativa objeto de feedback (se atribuído)
            if (Colect != null) Colect.SetActive(true);

            // Aqui você pode fazer outras coisas: incrementar inventário, tocar som, etc.

            // Remove a chave da cena
            Destroy(gameObject);
        }
    }
}