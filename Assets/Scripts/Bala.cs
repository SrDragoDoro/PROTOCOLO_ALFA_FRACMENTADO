using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float velocidad = 15f;
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
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

 

        Debug.Log("Daño aplicado: " + daño);

        Destroy(gameObject);
    }
}