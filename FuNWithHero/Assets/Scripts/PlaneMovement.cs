using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb2d;

    [Tooltip("Plane to Move")]
    public GameObject plane;

    [Tooltip("Initial speed")]
    public float speed = 25f;

    private float rotation;

    void Awake()
    {
        rb2d = plane.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        rotation = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            plane.transform.up = Vector2.Reflect(plane.transform.up, collision.GetContact(0).normal);
        }
    }

    private void FixedUpdate()
    {
        rb2d.MoveRotation(rb2d.rotation + rotation);
        rotation = 0;
        rb2d.MovePosition(rb2d.position + (Vector2)(plane.transform.TransformDirection(Vector3.up) * speed) * Time.deltaTime);
    }
}
