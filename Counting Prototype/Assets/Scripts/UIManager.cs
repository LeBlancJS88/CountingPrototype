using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text scoreText;

    private int levelCount;
    private int score;

    private void Start()
    {
        levelCount = 1;
        score = 0;
        UpdateLevelText();
        UpdateScoreText();
    }
    public void IncreaseLevel()
    {
        levelCount++;
        UpdateLevelText();
    }

    public void IncreaseScore()
    {
        score += 100;
        UpdateScoreText();
    }

    public void DeductScore()
    {
        score -= 50;
        UpdateScoreText();
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level: " + levelCount.ToString();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
