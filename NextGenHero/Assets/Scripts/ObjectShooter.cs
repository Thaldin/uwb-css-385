using UnityEngine;
using UnityEngine.UI;

public class ObjectShooter : MonoBehaviour
{
    [Header("Projectile Information")]

    [Tooltip("Object to Spawn")]
    public GameObject spawnObject;

    [Tooltip("Projectile Speed")]
    public float speed = 40f;

    [Header("Other Information")]
    public KeyCode keyToPress = KeyCode.Space;

    public Vector2 shotDirection = new Vector2(0f, 1f);

    [Tooltip("Fire Rate")]
    [Range(0.0f, 1.0f)]
    public float fireRate = 0.25f;

    private float lastShot = 0f;

    private UserInterface ui;

    // Start is called before the first frame update
    void Start()
    {
        lastShot -= fireRate;
        ui = GameObject.FindObjectOfType<UserInterface>();
        ui.SetPercentage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastShot < fireRate)
        {
            var timeLeft = fireRate - (Time.time - lastShot);
            ui.SetPercentage(timeLeft / fireRate * 100);
        }
        else
        {
            ui.SetPercentage(0);
        }

        if (Input.GetKey(keyToPress) && (Time.time - lastShot >= fireRate))
        {
            Vector2 actualEggDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * shotDirection;

            GameObject newObject = Instantiate(spawnObject);
            newObject.transform.position = this.transform.position;
            newObject.transform.eulerAngles = new Vector3(0f, 0f, Angle(actualEggDirection));
            newObject.tag = "Egg";

            // push the created objects, but only if they have a Rigidbody2D
            Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddForce(actualEggDirection * speed, ForceMode2D.Impulse);
            }
            ui.SetPercentage(100f);
            lastShot = Time.time;
        }
    }
    private float Angle(Vector2 inputVector)
    {
        if (inputVector.x < 0)
        {
            return (Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg * -1) - 360;
        }
        else
        {
            return -Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg;
        }
    }

    public void SetFireRate(float value)
    {
        fireRate = value;
    }
}
