using UnityEngine;

public class PlayerControlet : MonoBehaviour
{
    [SerializeField] private float speed = 80f;

    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");

        // Movimiento
        transform.position += Vector3.right * movimiento * speed * Time.deltaTime;

        // Giro visual
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