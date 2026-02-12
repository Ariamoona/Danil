using UnityEngine;
using VContainer;

public class ScoreTrigger : MonoBehaviour
{
    private bool scored = false;
    private PlayerService playerService;

    [Inject]
    public void Construct(PlayerService playerService)
    {
        this.playerService = playerService;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!scored && other.CompareTag("Player"))
        {
            scored = true;
            playerService.AddScore();
        }
    }
}
