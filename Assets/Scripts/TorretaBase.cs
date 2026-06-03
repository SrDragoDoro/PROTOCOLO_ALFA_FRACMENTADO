using UnityEngine;

public class TorretaBase : MonoBehaviour
{
    private enum TipoTorreta
    {
        None,
        Gun,
        Franco,
        Canon
    }


    [SerializeField] private TipoTorreta tipoTorreta;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float alcance;
    [SerializeField] private float daño;
    [SerializeField] private float tiempoEntreDisparos;
    private float temporizador;


    public GameObject Target;

    private void Awake()
    {
        //ConfigurarTorreta();
    }

   /* private void OnValidate()
    {
        ConfigurarTorreta();
    }*/

    private void Update()
    {
       /* temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreDisparos)
        {
            Disparar();
            temporizador = 0f;
        }*/
    }
    private void FixedUpdate()
    {
        ShootEnemy();
        DetectEnemies();
    }

    private void ConfigurarTorreta()
    {
        switch (tipoTorreta)
        {
            case TipoTorreta.None:
                alcance = 0f;
                daño = 0f;
                tiempoEntreDisparos = 9999f;
                break;

            case TipoTorreta.Gun:
                alcance = 8f;
                daño = 10f;
                tiempoEntreDisparos = 0.2f;
                break;

            case TipoTorreta.Franco:
                alcance = 20f;
                daño = 80f;
                tiempoEntreDisparos = 2f;
                break;

            case TipoTorreta.Canon:
                alcance = 12f;
                daño = 40f;
                tiempoEntreDisparos = 1f;
                break;
        }
    }

    private void Disparar()
    {
        GameObject nuevaBala = Instantiate(
            balaPrefab,
            puntoDisparo.position,
            puntoDisparo.rotation
        );

        Bala bala = nuevaBala.GetComponent<Bala>();

        if (bala != null)
        {
           // bala.Inicializar(daño);
        }
    }


    public void ShootEnemy()
    {
        if (Target == null) return;

        if(Vector3.Distance(Target.transform.position, transform.position) > alcance)
        {
            Target = null;
            return;
        }



        if(Target != null)
        {
            Vector3 dirShoot = (Target.transform.position - transform.localPosition).normalized;
            dirShoot.y = 0;
            transform.up = dirShoot;



            temporizador += Time.deltaTime;

            if (temporizador >= tiempoEntreDisparos)
            {
                ShootToShoot();
                temporizador = 0f;
            }

           
        }
    }
    public void ShootToShoot()
    {
        Vector3 dirShoot = (Target.transform.position - transform.position).normalized;

        GameObject bullet = Instantiate(balaPrefab, transform.position, Quaternion.identity);

        bullet.transform.up = dirShoot;
    }

    public void DetectEnemies()
    {
        if (Target != null) return;


        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, alcance);

        GameObject enemy = null;
        float minDistance = 0;

        foreach ( Collider2D coll in colls )
        {
            if(coll.CompareTag("Enemy"))
            {
                if(enemy == null)
                {
                    enemy = coll.gameObject;
                    minDistance = Vector3.Distance(enemy.transform.position, transform.position);
                }
                else
                {
                    if (Vector3.Distance(coll.gameObject.transform.position, transform.position) < minDistance)
                    {
                        enemy = coll.gameObject;
                        minDistance = Vector3.Distance(enemy.transform.position, transform.position);
                    }

                }
            }
        }
        Target = enemy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}