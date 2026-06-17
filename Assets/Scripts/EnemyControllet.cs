using UnityEngine;
public class Enemy : MonoBehaviour
{
    [Header("Vida del enemigo")]
    [SerializeField] private float vidaMaxima = 30f;
    private float vidaActual;

    [Header("Movimiento")]
    [SerializeField] private float velocidad = 2f;

    [Header("Detección de amenazas")]
    [SerializeField] private float radioDeteccion = 4f;
    [SerializeField] private string tagTorreta = "Torreta";
    [SerializeField] private string tagPlayer = "Player";
    [SerializeField] private string tagCasa = "Casa";

    [Header("Ataque cuerpo a cuerpo")]
    [SerializeField] private float danioAtaque = 10f;
    [SerializeField] private float rangoAtaque = 1f;
    [SerializeField] private float tiempoEntreAtaques = 1f;

    private Transform objetivoCasa;     // La casa a la que siempre intentará llegar si no hay amenazas
    private Transform objetivoActual; // A quién está atacando o siguiendo ahora mismo
    private float temporizadorAtaque = 0f;

    private void Start()
    {
        vidaActual = vidaMaxima;

        GameObject casa = GameObject.FindGameObjectWithTag(tagCasa);
        if (casa != null)
        {
            objetivoCasa = casa.transform;
        }
        else
        {
            Debug.LogWarning("[Enemy] No se encontró ningún objeto con tag 'Casa' en la escena.");
        }
    }

    private void Update()
    {
        temporizadorAtaque -= Time.deltaTime;

        Transform amenaza = BuscarAmenazaMasCercana();

        if (amenaza != null)
        {
            objetivoActual = amenaza;
        }
        else
        {
            objetivoActual = objetivoCasa;
        }

        if (objetivoActual == null) return;

        float distancia = Vector2.Distance(transform.position, objetivoActual.position);

        if (distancia <= rangoAtaque)
        {
            // Está en rango de ataque: se queda quieto y golpea
            if (temporizadorAtaque <= 0f)
            {
                Atacar(objetivoActual);
                temporizadorAtaque = tiempoEntreAtaques;
            }
        }
        else
        {
            // Aún no está en rango: se mueve hacia el objetivo
            MoverHacia(objetivoActual.position);
        }
    }

    private Transform BuscarAmenazaMasCercana()
    {
        Collider2D[] resultados = Physics2D.OverlapCircleAll(transform.position, radioDeteccion);

        Transform masCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider2D col in resultados)
        {
            if (col.CompareTag(tagTorreta) || col.CompareTag(tagPlayer))
            {
                float distancia = Vector2.Distance(transform.position, col.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    masCercano = col.transform;
                }
            }
        }

        return masCercano;
    }

    private void MoverHacia(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    private void Atacar(Transform objetivo)
    {
        IDamageable danable = objetivo.GetComponent<IDamageable>();
        if (danable != null)
        {
            danable.RecibirDanio(danioAtaque);
        }
        else
        {
            Debug.LogWarning("[Enemy] El objetivo '" + objetivo.name + "' no tiene un componente IDamageable.");
        }
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
        Destroy(gameObject);
    }

    // Dibuja el radio de detección en el Editor para verlo mientras lo ajustas
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}

/// <summary>
/// Interfaz simple para que cualquier objeto (Casa, Torreta, Player) pueda recibir daño.
/// Cualquier script de vida que ya tengas o vayas a crear debe implementar este método.
/// Ejemplo: public class VidaJugador : MonoBehaviour, IDamageable { ... }
/// </summary>
public interface IDamageable
{
    void RecibirDanio(float cantidad);
}