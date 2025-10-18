using UnityEngine;

public class ParallaxLimitado : MonoBehaviour
{
    public Transform cameraTransform;   // A câmera
    public float fatorParallax = 0.5f;  // Velocidade da camada
    public float limiteInicio;          // Posição X onde o movimento começa
    public float limiteFim;             // Posição X onde o movimento termina

    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float deslocamentoX = (cameraTransform.position.x - posInicial.x) * fatorParallax;
        float novaPosX = posInicial.x + deslocamentoX;

        // Limita o movimento entre os pontos definidos
        novaPosX = Mathf.Clamp(novaPosX, limiteInicio, limiteFim);

        transform.position = new Vector3(novaPosX, transform.position.y, transform.position.z);
    }
}