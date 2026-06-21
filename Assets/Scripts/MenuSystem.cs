using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void CambiarEscena(string EscenaJuego)
    {
        SceneManager.LoadScene(EscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Menu System EXIT");
        Application.Quit();
    }
}