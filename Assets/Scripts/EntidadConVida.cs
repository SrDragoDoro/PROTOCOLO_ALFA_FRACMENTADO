using UnityEngine;

public abstract class EntidadConVida : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [SerializeField] protected float vidaMaxima = 50f;

    protected float vidaActual;
    protected bool estaMuerto = false;

    protected virtual void Awake()
    {
        vidaActual = vidaMaxima;
    }

    public virtual void RecibirDanio(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;

        Debug.Log(gameObject.name + " recibió daño: " + cantidad + " | Vida: " + vidaActual);

        if (vidaActual <= 0f)
        {
            vidaActual = 0f;
            estaMuerto = true;
            Morir();
        }
    }

    protected abstract void Morir();
}