using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;

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
    }
}