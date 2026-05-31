using UnityEngine;

public class PlayerControlet : MonoBehaviour
{

[SerializeField] private float speed = 80f;
    void Start()
    {

    
    }


    void Update()
    {
        float velocidadX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 position = transform.position;
        transform.position = new Vector3(velocidadX + position.x, position.y, position.z);
    }
}
