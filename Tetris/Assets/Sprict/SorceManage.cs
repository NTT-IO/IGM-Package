using System.Collections;
using TMPro;
using UnityEngine;

public class SorceManage : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public float scoreAnimationDuration = 1f;
    public float scoreUpdateInterval = 0.05f;
    private int currentDisplayScore;
    public static int actualScore = 0;
    [SerializeField] private Board board;
    private void Start()
    {
        UpdateScoreDisplay(0);
        if (board != null)
        {
            board.onLinesCleared += HandleLineCleared;
        }
    }
    private void HandleLineCleared(int lines)
    {
        int scoreToAdd = (int)(MosterData.hpBouns * MosterData.speedBouns * (lines == 4 ? 100 : lines * 10));
        AddScore(scoreToAdd);
        ColorLerpUtility.UpdateTextColor(actualScore, 0f, 1000, Color.black, Color.red, scoreText);
    }
    private void AddScore(int scoreToAdd)
    {
        actualScore += scoreToAdd;
        UpdateScoreDisplay(actualScore);
        StartCoroutine(AnimateScore());
        if (actualScore >= 1000)
        {
            board.ifFinal = true;
            board.ClearLine();
        }
    }
    private IEnumerator AnimateScore()
    {
        float animationtime = 0f;
        int startScore = currentDisplayScore;
        while (animationtime < scoreAnimationDuration)
        {
            animationtime += scoreUpdateInterval;
            float progress = animationtime / scoreAnimationDuration;
            currentDisplayScore = (int)Mathf.Lerp(startScore, actualScore, progress);
            UpdateScoreDisplay(currentDisplayScore);
            yield return new WaitForSeconds(scoreUpdateInterval);
        }
        currentDisplayScore = actualScore;
        UpdateScoreDisplay(currentDisplayScore);
    }
    private void UpdateScoreDisplay(int score)
    {
        scoreText.text = score+"";
    }
    public void ResetScore()
    {
        actualScore = 0;
        currentDisplayScore = 0;
        UpdateScoreDisplay(0);
    }
}