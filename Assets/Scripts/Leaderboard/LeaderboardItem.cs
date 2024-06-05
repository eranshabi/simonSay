using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image image;

    public void SetNameText(string name)
    {
        nameText.text = name;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void MarkAsNew()
    {
        image.color = new Color(222f / 255f, 60f / 255f, 137f / 255f, 1); // Purple color to mark new records
    }
}
