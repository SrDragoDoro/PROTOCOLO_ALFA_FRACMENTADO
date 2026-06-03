using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{

    public void jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

 
    public void Salir()
    {
        Debug.Log("Menu System   EXIT ");
        Application.Quit();
    }
}
