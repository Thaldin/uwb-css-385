using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public float eggLifeInSeconds = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Can't be a negative lifetime
        if (eggLifeInSeconds < 0f)
        {
            eggLifeInSeconds = 1.0f;
        }

        // We want to destroy the egg after the specified time.
        Destroy(this.gameObject, eggLifeInSeconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
