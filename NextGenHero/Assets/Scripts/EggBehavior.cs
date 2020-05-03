using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    private UserInterface ui;

    // Start is called before the first frame update
    void Start()
    {
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
