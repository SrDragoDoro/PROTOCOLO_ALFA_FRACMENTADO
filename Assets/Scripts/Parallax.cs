using UnityEngine;

public class Parallax : MonoBehaviour
{
    //La camara que vamos a seguir
    [SerializeField]
    private Transform targetCamera;
    //-> que tan rapido se mueve este objeto en relacion a mi camara
    [SerializeField,Range(-1,1)]
    private float efectoParallax;
    [SerializeField]
    private float longitudSprite;
    private float posicionInicial;
    void Start()
    {
        posicionInicial = transform.position.x;
    }
    //->upadte se ejecuta n cantidad de veces isn restriccion
    //-> FixedUpdate se ejecuta 60 fps
    //->Late update se ejecuta despues del movimiento
    private void LateUpdate()
    {
        float distanciaParallax = targetCamera.position.x * efectoParallax;

        transform.position = new Vector3(posicionInicial + distanciaParallax ,transform.position.y,transform.position.z);

        float distanciaTemporal = targetCamera.position.x * (1 - efectoParallax);

        if(distanciaTemporal >  posicionInicial + longitudSprite)
        {
            posicionInicial += longitudSprite;
        }
        else if(distanciaTemporal < posicionInicial - longitudSprite)
        {
            posicionInicial -= longitudSprite;
        }
    }
}