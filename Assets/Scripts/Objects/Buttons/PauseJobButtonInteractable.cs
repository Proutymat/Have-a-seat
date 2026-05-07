using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class PauseJobButtonInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    [SerializeField] private DesktopSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private CinemachineCamera _chairCamera;
    
    public void Interact()
    {
        _scene.PauseWork();
        _player.ExitChair(_chairCamera);
    }

    public bool CanInteract()
    {
        return _player.State == PlayerState.Sitting;
    }
}
