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


    private void Awake()
    {
        ConfigurarTorreta();
    }

    private void OnValidate()
    {
        ConfigurarTorreta();
    }

    private void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreDisparos)
        {
            Disparar();
            temporizador = 0f;
        }
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
            bala.Inicializar(daño);
        }
    }
}