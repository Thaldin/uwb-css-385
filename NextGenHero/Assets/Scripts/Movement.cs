using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [HideInInspector]
    private Rigidbody2D rb2d;

    public GameObject player;

    [Header("Movement")]

    [Tooltip("Hero will follow the mouse pointer.")]
    public bool followMouse = false;

    [Tooltip("Initial speed")]
    public float speed = 1f;

    [Tooltip("How fast speed changes")]
    public float speedDelta = 0.5f;

    [Tooltip("How fast the rotation is applied")]
    [Min(0)]
    public float rotationDelta = 2.0f;

    private float rotation = 0f;

    private UserInterface ui;

    void Awake()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
        ui = FindObjectOfType<UserInterface>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        // Handle negative rotation delta
        if (rotationDelta < 0)
        {
            rotationDelta = 1.0f;
        }

        ui.ToggleInput(followMouse);
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            rotation = 0;
            player.transform.up = Vector2.Reflect(player.transform.up, collision.GetContact(0).normal);
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            var id = collision.gameObject.GetComponent<PlaneBehavior>().PlaneId;
            var worldUtils = FindObjectOfType<WorldUtils>();
            worldUtils.EnemyCollided(id);
            ui.EnemyCollided();
        }
    }

    private void FixedUpdate()
    {
        HandleInput();
        MoveHero();
    }

    private void MoveHero()
    {
        if (followMouse)
        {
            var camera = GameObject.FindObjectOfType<Camera>();
            var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            rb2d.MovePosition(mousePos);
            rotation = 0;
        }
        else
        {
            rb2d.MovePosition(rb2d.position + (Vector2)(player.transform.TransformDirection(Vector3.up) * speed) * Time.fixedDeltaTime);
        }

        if (rotation != 0)
        {
            rb2d.MoveRotation(rb2d.rotation + rotation);
            rotation = 0;
        }
    }

    private void HandleInput()
    {
        // Handle velocity
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speed += speedDelta;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speed -= speedDelta;
        }

        if (Input.GetKey(KeyCode.P))
        {
            speed = 0f;
        }

        // Handle rotation
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

        // Handle Mousemode Toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            followMouse = !followMouse;
            ui.ToggleInput(followMouse);
        }
    }
}
