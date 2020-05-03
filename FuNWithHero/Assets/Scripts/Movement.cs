using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb2d;

    public GameObject player;

    [Header("Movement")]

    [Tooltip("Initial speed")]
    public float speed = 1f;

    [Tooltip("How fast speed changes")]
    public float speedDelta = 0.5f;

    void Awake()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            player.transform.up = Vector2.Reflect(player.transform.up, collision.GetContact(0).normal);
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + (Vector2)(player.transform.TransformDirection(Vector3.up) * speed) * Time.fixedDeltaTime);
    }
}
