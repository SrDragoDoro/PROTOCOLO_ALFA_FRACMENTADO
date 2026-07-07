using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IWinCondition
{
    public static GameManager Instance;

    [Header("Victoria")]
    [SerializeField] private int puntosParaGanar = 100;
    [SerializeField] private GameObject imagenVictoria;
    [SerializeField] private float tiempoAntesDeMenu = 3f;
    [SerializeField] private int escenaMenuIndex = 0;

    private int puntosActuales = 0;
    private bool juegoTerminado = false;

    public enum GameState { Play, Pause, Win, Lose }
    public GameState CurrentState = GameState.Play;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (imagenVictoria != null)
            imagenVictoria.SetActive(false);

        int mejorPuntaje = PlayerPrefs.GetInt("MejorPuntaje", 0);
        Debug.Log("Mejor puntaje anterior: " + mejorPuntaje);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void AgregarPuntos(int puntos)
    {
        if (juegoTerminado) return;

        puntosActuales += puntos;
        Debug.Log("Puntos: " + puntosActuales + "/" + puntosParaGanar);

        if (CondicionCumplida())
            Ganar();
    }

    public bool CondicionCumplida() => puntosActuales >= puntosParaGanar;

    private void Ganar()
    {
        juegoTerminado = true;
        CurrentState = GameState.Win;

        int mejorPuntaje = PlayerPrefs.GetInt("MejorPuntaje", 0);
        if (puntosActuales > mejorPuntaje)
        {
            PlayerPrefs.SetInt("MejorPuntaje", puntosActuales);
            PlayerPrefs.Save();
        }

        if (imagenVictoria != null)
            imagenVictoria.SetActive(true);

        Time.timeScale = 0f;
        StartCoroutine(EsperarYVolverAlMenu());
        
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayVictoria();
    }

    private IEnumerator EsperarYVolverAlMenu()
    {
        yield return new WaitForSecondsRealtime(tiempoAntesDeMenu);
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenuIndex);
    }

    public void TogglePause()
    {
        if (juegoTerminado) return;

        if (CurrentState == GameState.Play)
        {
            CurrentState = GameState.Pause;
            Time.timeScale = 0f;
        }
        else if (CurrentState == GameState.Pause)
        {
            CurrentState = GameState.Play;
            Time.timeScale = 1f;
        }
    }
}