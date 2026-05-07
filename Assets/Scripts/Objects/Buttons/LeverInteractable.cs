using Sirenix.OdinInspector;
using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    [SerializeField] private DesktopSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private HandController _hand;

    public void Interact()
    {
       _scene.ActivateLever();
    }

    public bool CanInteract()
    {
        return _player.State == PlayerState.Sitting;
    }
}
