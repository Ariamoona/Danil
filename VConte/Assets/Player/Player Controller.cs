using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isDead = false;

    private PlayerService playerService;

    [Inject]
    public void Construct(PlayerService playerService)
    {
        this.playerService = playerService;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        isDead = true;
        playerService.PlayerDied();
    }
}
