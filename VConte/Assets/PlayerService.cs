using UnityEngine;

public class PlayerService
{
    private readonly GameManager gameManager;
    private readonly ScoreManager scoreManager;

    public PlayerService(GameManager gameManager, ScoreManager scoreManager)
    {
        this.gameManager = gameManager;
        this.scoreManager = scoreManager;
    }

    public void PlayerDied()
    {
        gameManager.GameOver();
    }

    public void AddScore()
    {
        scoreManager.AddScore();
    }
}
