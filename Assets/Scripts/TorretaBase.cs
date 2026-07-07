using UnityEngine;

public class TorretaBase : MonoBehaviour
{
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float alcance = 8f;
    [SerializeField] private float daño = 10f;
    [SerializeField] private float tiempoEntreDisparos = 0.5f;
    private float temporizador;

    private GameObject target;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Set(TurretData data)
    {
        alcance = data.TurrentRange;
        daño = data.BulletDamage;
        tiempoEntreDisparos = data.Cadencia;
    }

    private void FixedUpdate()
    {
        BuscarTarget();
        Disparar();
    }

    private void BuscarTarget()
    {
        if (target != null &&
            target.activeInHierarchy &&
            Vector3.Distance(transform.position, target.transform.position) <= alcance)
            return;

        target = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, alcance))
        {
            if (!col.CompareTag("Enemy")) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                target = col.gameObject;
            }
        }
    }

    private void Disparar()
    {
        if (target == null || balaPrefab == null) return;

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        bool enemyALaIzquierda = dir.x < 0;

        if (sr != null)
            sr.flipX = enemyALaIzquierda;

        float anguloFinal = enemyALaIzquierda
            ? Mathf.Clamp(180f - angulo, 90f, 270f)
            : Mathf.Clamp(angulo, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, 0f, anguloFinal);

        temporizador += Time.fixedDeltaTime;
        if (temporizador < tiempoEntreDisparos) return;
        temporizador = 0f;

        Transform spawn = puntoDisparo != null ? puntoDisparo : transform;
        float anguloDisparo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject bulletObj = Instantiate(
            balaPrefab,
            spawn.position,
            Quaternion.Euler(0f, 0f, anguloDisparo)
        );

        // Sonido de disparo de torreta
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayDisparoTorreta();

        Bala bala = bulletObj.GetComponent<Bala>();
        if (bala != null)
            bala.Inicializar(daño);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}