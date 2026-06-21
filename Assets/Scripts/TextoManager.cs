using UnityEngine;
using TMPro;
using System.Collections;

public class MostrarTextoTMP : MonoBehaviour
{
    [Header("Texto TMP")]
    [SerializeField] private TextMeshProUGUI textoMensaje;

    [Header("Configuración")]
    [SerializeField] private string mensaje = "Hola, este es el mensaje";
    [SerializeField] private float tiempoVisible = 3f;

    private Coroutine rutinaTexto;

    private void Start()
    {
        if (textoMensaje != null)
            textoMensaje.gameObject.SetActive(false);
    }

    public void MostrarTexto()
    {
        if (rutinaTexto != null)
            StopCoroutine(rutinaTexto);

        rutinaTexto = StartCoroutine(MostrarTextoTemporal());
    }

    private IEnumerator MostrarTextoTemporal()
    {
        textoMensaje.text = mensaje;
        textoMensaje.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiempoVisible);

        textoMensaje.gameObject.SetActive(false);
    }
}