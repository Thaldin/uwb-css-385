using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private const float SHOTBAR_SIZE = 200f;

    [Tooltip("Image that represents the shot timer")]
    public Image shotBar;

    [Tooltip("Text object that will show egg count")]
    public Text eggCounter;
    private int eggCount = 0;

    [Tooltip("Text object that will show the enemies shot")]
    public Text enemyShot;
    private int enemiesShot = 0;

    [Tooltip("Text object that will show the enemies collied")]
    public Text enemyCollided;
    private int enemiesCollided = 0;

    [Tooltip("Text object that will show the waypoints sequence")]
    public Text wayPointSeq;

    [Tooltip("Text object that will show the waypoints visibility")]
    public Text wayPointVis;

    [Tooltip("Text object that will show the waypoints visibility")]
    public Text inputMode;

    public void SetPercentage(float percent)
    {
        var rect = shotBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(SHOTBAR_SIZE * (percent / 100), rect.sizeDelta.y);
    }

    public void EggCountInc()
    {
        ++eggCount;
        eggCounter.text = eggCount.ToString();
    }

    public void EggCountDec()
    {
        --eggCount;
        eggCounter.text = eggCount.ToString();
    }

    public void EnemyCollided()
    {
        ++enemiesCollided;
        enemyCollided.text = enemiesCollided.ToString();
    }

    public void EnemyShot()
    {
        ++enemiesShot;
        enemyShot.text = enemiesShot.ToString();
    }

    public void ToggleWayPointVis(bool visible)
    {
        if (visible)
        {
            wayPointVis.text = "VIS";
        }
        else
        {
            wayPointVis.text = "HDN";
        }
    }

    public void ToggleWayPointSeq(bool sequential)
    {
        if (sequential)
        {
            wayPointSeq.text = "SEQ";
        }
        else
        {
            wayPointSeq.text = "RND";
        }
    }

    public void ToggleInput(bool mouseEnabled)
    {
        if (mouseEnabled)
        {
            inputMode.text = "Mouse";
        }
        else
        {
            inputMode.text = "Keyboard";
        }
    }
}
