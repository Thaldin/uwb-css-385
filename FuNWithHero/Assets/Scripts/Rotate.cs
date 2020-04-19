using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotate : MonoBehaviour
{
    public GameObject player;
    public float rotationDelta = 2.0f;

    private float rotation = 0f;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = player.GetComponent<Rigidbody2D>();

        // Handle negative rotation
        if (rotationDelta < 0)
        {
            rotationDelta = 1.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Wall"))
        {
            rotation = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rotation = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotation += rotationDelta;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotation -= rotationDelta;
        }
    }

    public void FixedUpdate()
    {
        rb2d.MoveRotation(rb2d.rotation + rotation * Time.deltaTime);
    }
}

