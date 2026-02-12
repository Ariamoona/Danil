using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(gameManager);
        builder.RegisterComponent(scoreManager);

        builder.Register<PlayerService>(Lifetime.Singleton);
    }
}
