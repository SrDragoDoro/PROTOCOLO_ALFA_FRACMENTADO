using UnityEngine;

public class CuerpoTorreta : MonoBehaviour, IDamageable
{
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;

    private GameObject cabeza;
    private CartelConstruccion cartel;

    public void Inicializar(GameObject cabezaRef, CartelConstruccion cartelRef)
    {
        cabeza = cabezaRef;
        cartel = cartelRef;
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (cabeza != null)
            Destroy(cabeza);

        if (cartel != null)
            cartel.OnTorretaDestruida();

        Destroy(gameObject);
    }
}