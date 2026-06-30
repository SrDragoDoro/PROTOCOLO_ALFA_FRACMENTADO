using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlet : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 20f;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float danioBala = 10f;

    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction pauseAction;
    private InputAction shootAction;

    private float movimiento;
    private bool facingRight = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("Falta el componente PlayerInput.");
            enabled = false;
            return;
        }

        moveAction = playerInput.actions["Move"];
        pauseAction = playerInput.actions["Pause"];
        shootAction = playerInput.actions["Shoot"]; // Crea esta acción o cambia el nombre
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        if (pauseAction != null)
            pauseAction.performed += OnPause;

        if (shootAction != null)
            shootAction.performed += OnShoot;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        if (pauseAction != null)
            pauseAction.performed -= OnPause;

        if (shootAction != null)
            shootAction.performed -= OnShoot;
    }

    private void Update()
    {
        transform.position += Vector3.right * movimiento * speed * Time.deltaTime;

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

    private void OnMove(InputAction.CallbackContext ctx)
    {
        movimiento = ctx.ReadValue<Vector2>().x;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.TogglePause();
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        Disparar();
    }

    private void Disparar()
    {
        if (bulletPrefab == null)
            return;

        Vector3 spawnPos = bulletSpawnPoint != null
            ? bulletSpawnPoint.position
            : transform.position;

        Quaternion rotacion = facingRight
            ? Quaternion.identity
            : Quaternion.Euler(0f, 180f, 0f);

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, rotacion);

        Bala bala = bullet.GetComponent<Bala>();

        if (bala != null)
            bala.Inicializar(danioBala);
    }
}