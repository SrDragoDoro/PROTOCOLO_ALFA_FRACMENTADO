using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject ToolTip;
    public GameObject OptionPanel;


    public List<TurretData> Opciones;

    public Transform SpawnPoint;
    public TorretaBase TurretPrefab;

    public TorretaBase CurrentTorre = null;

    void Start()
    {
        ToolTip.SetActive(false);
        OptionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ToolTip.SetActive(true);
            OptionPanel.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ToolTip.SetActive(false);
            OptionPanel.SetActive(false);
        }
    }

    public void CreateTurret(int pos)
    {
       TorretaBase turret =  Instantiate(TurretPrefab, SpawnPoint.position, Quaternion.identity, SpawnPoint.transform);
        CurrentTorre= turret;
        CurrentTorre.Set(Opciones[pos]);
        Debug.Log("___");
    }
}
