using UnityEngine;
using TMPro;

public class CartelConstruccion : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject textoPresionaE;
    [SerializeField] private GameObject canvasBotones;

    [Header("Punto de spawn")]
    [SerializeField] private Transform puntoSpawn;
    [SerializeField] private Vector3 offsetCabeza = new Vector3(0f, 1f, 0f);

    private bool playerDentro = false;
    private GameObject cuerpoActual;
    private GameObject cabezaActual;

    private void Start()
    {
        textoPresionaE.SetActive(false);
        canvasBotones.SetActive(false);
    }

    private void Update()
    {
        if (!playerDentro) return;

        if (Input.GetKeyDown(KeyCode.E) && !canvasBotones.activeSelf)
        {
            textoPresionaE.SetActive(false);
            canvasBotones.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectó: " + other.gameObject.name + " | Tag: " + other.tag);
        
        if (!other.CompareTag("Player")) return;

        playerDentro = true;

        if (cuerpoActual == null)
            textoPresionaE.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerDentro = false;
        textoPresionaE.SetActive(false);
        canvasBotones.SetActive(false);
    }

    public void ConstruirTorreta(GameObject prefabCuerpo, GameObject prefabCabeza, TurretData datos)
    {
        // Destruir torreta anterior si existe
        if (cuerpoActual != null) Destroy(cuerpoActual);
        if (cabezaActual != null) Destroy(cabezaActual);

        Vector3 posSpawn = puntoSpawn != null ? puntoSpawn.position : transform.position;

        // Instanciar Cuerpo
        cuerpoActual = Instantiate(prefabCuerpo, posSpawn, Quaternion.identity);

        // Instanciar Cabeza con offset
        cabezaActual = Instantiate(prefabCabeza, posSpawn + offsetCabeza, Quaternion.identity);

        // Inicializar el Cuerpo pasándole referencia a la Cabeza y al Cartel
        CuerpoTorreta cuerpo = cuerpoActual.GetComponent<CuerpoTorreta>();
        if (cuerpo != null)
        {
            cuerpo.Inicializar(cabezaActual, this);
        }

        // Inicializar la Cabeza con los datos de la torreta
        TorretaBase cabeza = cabezaActual.GetComponent<TorretaBase>();
        if (cabeza != null)
        {
            cabeza.Set(datos);
        }

        // Cerrar el menú
        canvasBotones.SetActive(false);
    }

    // Llamado por CuerpoTorreta cuando muere — reactiva el cartel
    public void OnTorretaDestruida()
    {
        cuerpoActual = null;
        cabezaActual = null;

        if (playerDentro)
            textoPresionaE.SetActive(true);
    }

}