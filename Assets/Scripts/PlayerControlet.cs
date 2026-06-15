using UnityEngine;
using UnityEngine.InputSystem; // Obligatorio

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControlet : MonoBehaviour
{
    [SerializeField] private float speed = 20f; 

    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;
    private InputAction moveAction;
    
    private bool facingRight = true;
    private float movimiento; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Buscamos el componente Player Input en el personaje
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            // Buscamos la acción "Move" dentro del mapa actual
            moveAction = playerInput.actions.FindAction("Move");
        }
        else
        {
            Debug.LogError("¡Falta el componente Player Input en este objeto!");
        }
    }

    void Update()
    {
        // Si encontramos la acción, leemos directamente su valor en X
        if (moveAction != null)
        {
            movimiento = moveAction.ReadValue<Vector2>().x;
        }

        // Movimiento horizontal
        transform.position += Vector3.right * movimiento * speed * Time.deltaTime;

        // Giro visual 
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
    }
}