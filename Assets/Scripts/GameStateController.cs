using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance;

    public GameStage CurrentStage { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetStage(GameStage stage)
    {
        CurrentStage = stage;
    }
}