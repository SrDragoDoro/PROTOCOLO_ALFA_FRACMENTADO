using UnityEngine;

public class BotonTorreta : MonoBehaviour
{
    [SerializeField] private CartelConstruccion cartel;
    [SerializeField] private GameObject prefabCuerpo;
    [SerializeField] private GameObject prefabCabeza;
    [SerializeField] private TurretData datos;

    public void AlPresionar()
    {
        if (cartel == null)
        {
            Debug.LogWarning("BotonTorreta: no hay cartel asignado.");
            return;
        }

        cartel.ConstruirTorreta(prefabCuerpo, prefabCabeza, datos);
    }
}