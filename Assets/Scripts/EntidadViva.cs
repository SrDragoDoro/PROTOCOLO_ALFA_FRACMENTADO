using UnityEngine;

public abstract class EntidadViva : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [SerializeField] protected float vidaMaxima = 100f;
    protected float vidaActual;

    protected virtual void Start()
    {
        vidaActual = vidaMaxima;
    }

    public virtual void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log(gameObject.name + " recibió daño: " + cantidad + " | Vida: " + vidaActual);

        if (vidaActual <= 0f)
            Morir();
    }

    protected abstract void Morir();
}