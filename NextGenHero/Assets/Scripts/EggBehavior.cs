using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public float eggLifeInSeconds = 1.0f;

    private UserInterface ui;

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

        ui = FindObjectOfType<UserInterface>();
        ui.EggCountInc();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
            var id = collision.gameObject.GetComponent<PlaneBehavior>().PlaneId;
            var worldUtils = FindObjectOfType<WorldUtils>();
            worldUtils.EnemyHit(id);
            ui.EnemyShot();
        }
    }

    private void OnDestroy()
    {
        ui.EggCountDec();
    }
}
