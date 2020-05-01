using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldUtils : MonoBehaviour
{
    const int WayPointCount = 6;
    
    [Tooltip("Number of enemies in the world")]
    [Range(1, 25)]
    public int NumOfEnemies = 10;

    [Tooltip("Enemy to spawn")]
    public GameObject Enemy;

    [Tooltip("Waypoint to spawn")]
    public GameObject WayPoint;

    [Tooltip("Show or hide the waypoints")]
    public bool HideWayPoints = true;

    [Tooltip("Sequential or random waypoints")]
    public bool SequentialWayPoints = true;

    public ParticleSystem particles;
    public ParticleSystem particlesCol;

    private Bounds mWorldBounds;
    private List<GameObject> enemies;
    private List<GameObject> wayPoints;
    private UserInterface ui;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        ui = FindObjectOfType<UserInterface>();
        GetWorldBounds();

        // Initialize WayPoints
        wayPoints = new List<GameObject>();
        for (int i = 0; i < WayPointCount; i++)
        {
            var wp = Instantiate(WayPoint);
            wp.tag = "WayPoint";
            wp.GetComponent<WayPointBehavior>().WayPointId = i;
            wp.transform.position = GetSpawnPoint(wp, true);
            Sprite sprite = Resources.Load<Sprite>($"WayPoint_{i}");
            wp.GetComponent<SpriteRenderer>().sprite = sprite;
            wp.GetComponent<SpriteRenderer>().enabled = !HideWayPoints;
            wayPoints.Add(wp);
        }

        // Initialize Enemies
        enemies = new List<GameObject>();
        for (int i = 0; i < NumOfEnemies; i++)
        {
            var enemy = Instantiate(Enemy);
            enemy.tag = "Enemy";
            enemy.GetComponent<PlaneBehavior>().PlaneId = i;
            var target = Random.Range(0, 6);
            enemy.GetComponent<PlaneBehavior>().target = wayPoints[target];
            enemy.GetComponent<PlaneBehavior>().targetId = target;
            enemy.transform.position = GetSpawnPoint(enemy, false);
            enemies.Add(enemy);
        }

        ui.ToggleWayPointSeq(SequentialWayPoints);
        ui.ToggleWayPointVis(!HideWayPoints);
    }

    public GameObject GetWayPoint(int targetId)
    {
        var id = 0;

        if (SequentialWayPoints)
        {
            if (targetId == 5)
            {
                id = 0;
            }
            else
            {
                id = targetId + 1;
            }
        }
        else
        {
            do
            {
                id = Random.Range(0, 6);
            } while (id == targetId);
        }

        return wayPoints[id];
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Quit
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("TitleScreen");
        }

        // Handle Waypoint visibility
        if (Input.GetKeyDown(KeyCode.H))
        {
            HideWayPoints = !HideWayPoints;
            UpdateWayPoints();
        }

        // Handle Waypoint traversal
        if (Input.GetKeyDown(KeyCode.J))
        {
            SequentialWayPoints = !SequentialWayPoints;
            ui.ToggleWayPointSeq(SequentialWayPoints);
        }

        // Respawn waypoint
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var wp in wayPoints)
            {
                var newPos = GetSpawnPoint(wp, true);
                wp.transform.position = newPos;
                wp.GetComponent<WayPointBehavior>().origX = newPos.x;
                wp.GetComponent<WayPointBehavior>().origY = newPos.y;
            }
        }
    }

    public Vector3 GetSpawnPoint(GameObject gameObj, bool waypoint)
    {
        var x = mWorldBounds.min.x + UnityEngine.Random.value * mWorldBounds.size.x;
        var y = mWorldBounds.min.y + UnityEngine.Random.value * mWorldBounds.size.y;

        // Check for position vs. size of game object
        float diffX = (mWorldBounds.size.x / 2) - Mathf.Abs(x);
        float diffY = (mWorldBounds.size.y / 2) - Mathf.Abs(y);

        if (!waypoint)
        {
            var objBounds = gameObj.GetComponent<Renderer>().bounds;
            if (objBounds.size.x > diffX)
            {
                if (x > 0)
                {
                    x -= objBounds.size.x;
                }
                else
                {
                    x += objBounds.size.x;
                }
            }

            if (objBounds.size.y > diffY)
            {
                if (y > 0)
                {
                    y -= objBounds.size.y;
                }
                else
                {
                    y += objBounds.size.y;
                }
            }
        }
        else
        {
            if (20f > diffX)
            {
                if (x > 0)
                {
                    x -= 20f;
                }
                else
                {
                    x += 20f;
                }
            }

            if (20f > diffY)
            {
                if (y > 0)
                {
                    y -= 20f;
                }
                else
                {
                    y += 20f;
                }
            }
        }

        return new Vector3(x, y, 1.0f); ;
    }

    private void GetWorldBounds()
    {
        var sizeX = Camera.main.orthographicSize * Camera.main.aspect * 2;
        var sizeY = Camera.main.orthographicSize * 2;

        mWorldBounds.center = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
        mWorldBounds.size = new Vector3(sizeX, sizeY, 1.0f);
    }
    
    public void EnemyHit(int id)
    {
        var exp = Instantiate(particles) as ParticleSystem;
        exp.transform.position = enemies[id].transform.position;
        exp.Play();
        enemies[id].GetComponent<PlaneBehavior>().Respawn();
    }

    public void EnemyCollided(int id)
    {
        var col = Instantiate(particlesCol) as ParticleSystem;
        col.transform.position = enemies[id].transform.position;
        col.Play();
        enemies[id].GetComponent<PlaneBehavior>().Respawn();
    }

    private void UpdateWayPoints()
    {
        foreach(GameObject wp in wayPoints)
        {
            wp.GetComponent<SpriteRenderer>().enabled = !HideWayPoints;
        }
        ui.ToggleWayPointVis(!HideWayPoints);
    }

    public void WayPointHit(int id)
    {
        var wpColor = wayPoints[id].GetComponent<SpriteRenderer>().color;
        wpColor.a -= 0.25f;
        wayPoints[id].GetComponent<SpriteRenderer>().color = wpColor;

        if (wpColor.a == 0f)
        {
            wayPoints[id].GetComponent<WayPointBehavior>().RespawnWayPoint();
            wpColor.a = 1.0f;
            wayPoints[id].GetComponent<SpriteRenderer>().color = wpColor;
        }
    }

    public bool WayPointsVisible()
    {
        return !HideWayPoints;
    }

    public bool WayPointsSequential()
    {
        return SequentialWayPoints;
    }

}
