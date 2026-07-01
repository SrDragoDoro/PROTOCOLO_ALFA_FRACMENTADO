using UnityEngine;

public class Torreta : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float dañoBala = 10f;
    [SerializeField] private float tiempoEntreDisparos = 1f;

    [Header("Detección")]
    [SerializeField] private float rango = 8f;
    [SerializeField] private string enemyTag = "Enemy";

    private Transform objetivoActual;
    private float temporizadorDisparo = 0f;

    private void Update()
    {
        temporizadorDisparo += Time.deltaTime;

        BuscarEnemigoMasCercano();

        if (objetivoActual != null)
        {
            ApuntarAlObjetivo();

            if (temporizadorDisparo >= tiempoEntreDisparos)
            {
                Disparar();
                temporizadorDisparo = 0f;
            }
        }
    }

    private void BuscarEnemigoMasCercano()
    {
        Collider2D[] enemigosDetectados = Physics2D.OverlapCircleAll(transform.position, rango);

        Transform enemigoMasCercano = null;
        float distanciaMasCorta = Mathf.Infinity;

        foreach (Collider2D enemigo in enemigosDetectados)
        {
            if (enemigo.CompareTag(enemyTag))
            {
                float distancia = Vector2.Distance(transform.position, enemigo.transform.position);

                if (distancia < distanciaMasCorta)
                {
                    distanciaMasCorta = distancia;
                    enemigoMasCercano = enemigo.transform;
                }
            }
        }

        objetivoActual = enemigoMasCercano;
    }

    private void ApuntarAlObjetivo()
    {
        Vector2 direccion = objetivoActual.position - transform.position;

        transform.right = direccion;
    }

    private void Disparar()
    {
        if (balaPrefab == null)
        {
            Debug.LogWarning("No hay prefab de bala asignado.");
            return;
        }

        Vector3 posicionDisparo = puntoDisparo != null
            ? puntoDisparo.position
            : transform.position;

        GameObject nuevaBala = Instantiate(
            balaPrefab,
            posicionDisparo,
            Quaternion.identity
        );

        Vector2 direccion = objetivoActual.position - posicionDisparo;
        nuevaBala.transform.right = direccion.normalized;

        Bala bala = nuevaBala.GetComponent<Bala>();

        if (bala != null)
        {
            bala.Inicializar(dañoBala);
        }
        else
        {
            Debug.LogWarning("El prefab de bala no tiene el script Bala.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}