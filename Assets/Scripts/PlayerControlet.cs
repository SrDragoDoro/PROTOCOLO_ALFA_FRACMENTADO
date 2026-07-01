using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float danioBala = 10f;

    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction pauseAction;

    private bool facingRight = true;
    private float movimiento;

    private void Start()
    {
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
        // Movimiento
        if (moveAction != null)
        {
            movimiento = moveAction.ReadValue<Vector2>().x;

            transform.position +=
                Vector3.right *
                movimiento *
                speed *
                Time.deltaTime;
        }

        // Animación de flip según dirección
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

        // Pausa
        if (pauseAction != null &&
            pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.TogglePause();
        }

        // Disparo
        if (Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame)
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
}