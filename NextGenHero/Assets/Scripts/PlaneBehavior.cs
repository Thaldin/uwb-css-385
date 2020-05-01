using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    [HideInInspector]
    public int PlaneId;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public int targetId;

    [Tooltip("Initial speed")]
    public float speed = 25f;

    private Rigidbody2D rb2d;
    private float rotation;

    private WorldUtils worldUtils;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        worldUtils = FindObjectOfType<WorldUtils>();
        rotation = Random.Range(0f, 360f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            gameObject.transform.up = Vector2.Reflect(gameObject.transform.up, collision.GetContact(0).normal);
        }

        if (collision.gameObject.tag.Equals("WayPoint"))
        {
            var wp = collision.gameObject.GetComponent<WayPointBehavior>();
            if (wp.WayPointId == targetId)
            {
                target = worldUtils.GetWayPoint(targetId);
                targetId = target.gameObject.GetComponent<WayPointBehavior>().WayPointId;
            }
        }
    }

    private void FixedUpdate()
    {
        var newVector = target.transform.position - transform.position;
        var angle = Vector3.Angle(newVector, transform.up);
        rotation = angle * 3.0f * Time.fixedDeltaTime;
        rb2d.MoveRotation(rb2d.rotation + rotation);
        rb2d.MovePosition(rb2d.position + (Vector2)(gameObject.transform.TransformDirection(Vector3.up) * speed) * Time.fixedDeltaTime);
        
    }

    public void Respawn()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        var collider = gameObject.GetComponent<BoxCollider2D>();

        collider.enabled = false;
        renderer.enabled = false;

        var newSpawn = worldUtils.GetSpawnPoint(gameObject, false);
        gameObject.transform.position = newSpawn;
        rotation = Random.Range(0f, 360f);

        collider.enabled = true;
        renderer.enabled = true;
    }
}
