using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class PauseJobInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    //[SerializeField] private DesktopSceneController _controller;
    [SerializeField] private PlayerStateController _playerStateController;
    [SerializeField] private CinemachineCamera _chairCamera;
    
    public void Interact()
    {
        _playerStateController.ExitChair(_chairCamera);
    }

    public bool CanInteract()
    {
        return _playerStateController.State == PlayerState.Sitting;
    }
}
