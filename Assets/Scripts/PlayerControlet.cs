using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControlet : MonoBehaviour, IDamageable
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 20f;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float danioBala = 10f;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 50f;
    private float vidaActual;

    [Header("Derrota")]
    [SerializeField] private GameObject imagenDerrota;
    [SerializeField] private int escenaMenuIndex = 1;
    [SerializeField] private float tiempoAntesDeMenu = 1f;

    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction pauseAction;

    private bool facingRight = true;
    private float movimiento;
    private bool muerto = false;

    private void Start()
    {
        vidaActual = vidaMaxima;

        if (imagenDerrota != null)
            imagenDerrota.SetActive(false);

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("¡Falta el componente PlayerInput!");
            return;
        }

        moveAction = playerInput.actions.FindAction("Move");
        pauseAction = playerInput.actions.FindAction("Pause");

        if (moveAction == null)
            Debug.LogError("No se encontró la acción 'Move'.");

        if (pauseAction == null)
            Debug.LogError("No se encontró la acción 'Pause'.");

        if (bulletPrefab == null)
            Debug.LogError("No se asignó el prefab de la bala.");
    }

    private void Update()
    {
        if (muerto) return;

        if (moveAction != null)
        {
            movimiento = moveAction.ReadValue<Vector2>().x;

            transform.position +=
                Vector3.right *
                movimiento *
                speed *
                Time.deltaTime;
        }

        if (spriteRenderer != null && movimiento != 0)
        {
            if (movimiento < 0 && facingRight)
            {
                spriteRenderer.flipX = true;
                facingRight = false;
            }
            else if (movimiento > 0 && !facingRight)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
            }
        }

        if (pauseAction != null && pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.TogglePause();
        }

        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = bulletSpawnPoint != null
            ? bulletSpawnPoint.position
            : transform.position;

        Quaternion rotacion = facingRight
            ? Quaternion.identity
            : Quaternion.Euler(0f, 180f, 0f);

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, rotacion);

        Bala bala = bullet.GetComponent<Bala>();
        if (bala != null)
        {
            bala.Inicializar(danioBala);
        }
        else
        {
            Debug.LogWarning("El prefab de bala no tiene el script Bala.");
        }
    }

    public void RecibirDanio(float cantidad)
    {
        if (muerto) return;

        vidaActual -= cantidad;

        Debug.Log("Player recibió daño: " + cantidad + " | Vida actual: " + vidaActual);

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    private void Morir()
    {
        muerto = true;

        if (GameManager.Instance != null)
            GameManager.Instance.CurrentState = GameManager.GameState.Lose;

        if (imagenDerrota != null)
            imagenDerrota.SetActive(true);

        Time.timeScale = 0f;

        StartCoroutine(VolverAlMenu());
    }

    private IEnumerator VolverAlMenu()
    {
        yield return new WaitForSecondsRealtime(tiempoAntesDeMenu);

        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenuIndex);
    }
}