using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private Text scoreText;

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
