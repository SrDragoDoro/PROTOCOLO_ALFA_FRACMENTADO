using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Play,
        Pause,
        Win,
        Lose
    }

    public GameState CurrentState = GameState.Play;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Play)
        {
            CurrentState = GameState.Pause;
            Time.timeScale = 0;
        }
        else if (CurrentState == GameState.Pause)
        {
            CurrentState = GameState.Play;
            Time.timeScale = 1;
        }
    }
}