using UnityEngine;

public class EnemyControllet : EntidadViva, IPunteable
{
    [Header("Puntos de victoria")]
    [SerializeField] private int puntosVictoria = 1;

    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3f;

    [Header("Detección de amenazas")]
    [SerializeField] private float radioDeteccion = 4f;
    [SerializeField] private string tagTorreta = "Torreta";
    [SerializeField] private string tagPlayer = "Player";
    [SerializeField] private string tagCasa = "Casa";

    [Header("Ataque cuerpo a cuerpo")]
    [SerializeField] private float danioAtaque = 10f;
    [SerializeField] private float rangoAtaque = 1f;
    [SerializeField] private float tiempoEntreAtaques = 1f;

    private Transform objetivoCasa;
    private Transform objetivoActual;
    private float temporizadorAtaque = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject casa = GameObject.FindGameObjectWithTag(tagCasa);
        if (casa != null)
            objetivoCasa = casa.transform;
        else
            Debug.LogWarning("[Enemy] No se encontró ningún objeto con tag 'Casa'.");
    }

    private void Update()
    {
        temporizadorAtaque -= Time.deltaTime;

        Transform amenaza = BuscarAmenazaMasCercana();
        objetivoActual = amenaza != null ? amenaza : objetivoCasa;

        if (objetivoActual == null)
        {
            if (animator != null) animator.SetBool("Attack", false);
            return;
        }

        float distancia = Vector2.Distance(transform.position, objetivoActual.position);

        if (distancia <= rangoAtaque)
        {
            if (animator != null) animator.SetBool("Attack", true);

            if (temporizadorAtaque <= 0f)
            {
                Atacar(objetivoActual);
                temporizadorAtaque = tiempoEntreAtaques;
            }
        }
        else
        {
            if (animator != null) animator.SetBool("Attack", false);
            MoverHacia(objetivoActual.position);
            MirarHacia(objetivoActual.position);
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
                float dist = Vector2.Distance(transform.position, col.transform.position);
                if (dist < distanciaMinima)
                {
                    distanciaMinima = dist;
                    masCercano = col.transform;
                }
            }
        }
        return masCercano;
    }

    private void MoverHacia(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(
            transform.position, destino, velocidad * Time.deltaTime);
    }

    private void MirarHacia(Vector3 objetivo)
    {
        if (spriteRenderer == null) return;
        float dir = objetivo.x - transform.position.x;
        if (dir > 0f) spriteRenderer.flipX = false;
        else if (dir < 0f) spriteRenderer.flipX = true;
    }

    private void Atacar(Transform objetivo)
    {
        IDamageable danable = objetivo.GetComponent<IDamageable>();
        if (danable != null)
            danable.RecibirDanio(danioAtaque);
        else
            Debug.LogWarning("[Enemy] El objetivo '" + objetivo.name + "' no tiene IDamageable.");
    }

    protected override void Morir()
    {
        Debug.Log("Morir() llamado en: " + gameObject.name);
        Debug.Log("GameManager.Instance es null: " + (GameManager.Instance == null));
        
        try
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AgregarPuntos(puntosVictoria);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al agregar puntos: " + e.Message);
        }

        Destroy(gameObject);
    }

    public int GetPuntos() => puntosVictoria;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}