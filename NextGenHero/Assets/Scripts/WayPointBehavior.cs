using UnityEngine;

public class WayPointBehavior : MonoBehaviour
{
    [HideInInspector]
    public int WayPointId;
    [HideInInspector]
    public float origX;
    [HideInInspector]
    public float origY;

    private WorldUtils worldUtils;

    // Start is called before the first frame update
    void Start()
    {
        // Store original starting location
        origX = gameObject.transform.position.x;
        origY = gameObject.transform.position.y;

        // Get world utilities
        worldUtils = FindObjectOfType<WorldUtils>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Egg") && worldUtils.WayPointsVisible())
        {
            Destroy(collision.gameObject);
            var id = gameObject.GetComponent<WayPointBehavior>().WayPointId;
            worldUtils.WayPointHit(id);
        }
    }

    public void RespawnWayPoint()
    {
        int negX = Random.Range(0, 2);
        int negY = Random.Range(0, 2);
        float newX = Random.Range(1f, 15f);
        float newY = Random.Range(1f, 15f);

        if (negX == 1)
        {
            newX = -newX;
        }

        if (negY == 1)
        {
            newY = -newY;
        }

        gameObject.transform.position = new Vector3(origX + newX, origY + newY, 1f);
    }
}
