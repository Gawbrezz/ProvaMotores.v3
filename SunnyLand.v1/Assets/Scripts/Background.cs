using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float tamanho; // tamanho total do sprite em unidades
    public Transform cameraTransform;

    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (cameraTransform.position.x - transform.position.x >= tamanho)
        {
            transform.position += new Vector3(tamanho * 2, 0, 0);
        }
        else if (transform.position.x - cameraTransform.position.x >= tamanho)
        {
            transform.position -= new Vector3(tamanho * 2, 0, 0);
        }
    }
}
