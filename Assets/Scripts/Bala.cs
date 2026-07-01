using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float velocidad = 30f;
    [SerializeField] private float tiempoVida = 1f;
    [SerializeField] private float daño = 5f;

    public void Inicializar(float nuevoDaño)
    {
        daño = nuevoDaño;
    }

    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    private void Update()
    {
        //ransform.Translate(Vector2.right * velocidad * Time.deltaTime);

        transform.position += transform.right * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bala chocó con: " + other.gameObject.name + " | Tag: " + other.tag);

        if (!other.CompareTag("Enemy"))
            return;

        IDamageable danable = other.GetComponent<IDamageable>();
        if (danable != null)
        {
            danable.RecibirDanio(daño);
        }
        else
        {
            Debug.LogWarning("El objetivo no tiene un componente IDamageable.");
        }

        Destroy(gameObject);
    }
}