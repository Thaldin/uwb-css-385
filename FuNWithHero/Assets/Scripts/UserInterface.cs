using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private const float SHOTBAR_SIZE = 200f;

    [Tooltip("Image that represents the shot timer")]
    public Image shotBar;

    public void SetPercentage(float percent)
    {
        var rect = shotBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(SHOTBAR_SIZE * (percent / 100), rect.sizeDelta.y);
    }
}
