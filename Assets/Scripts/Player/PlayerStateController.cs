using UnityEngine;
using Unity.Cinemachine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerState State { get; private set; } = PlayerState.Free;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CinemachineCamera _playerCamera;

    private CinemachineCamera _currentInteractionCamera;
    
    public void EnterChair(CinemachineCamera chairCamera)
    {
        State = PlayerState.Sitting;

        _playerController.enabled = false;

        _currentInteractionCamera = chairCamera;

        _playerCamera.Priority = 0;
        chairCamera.Priority = 10;
    }

    public void ExitChair()
    {
        State = PlayerState.Free;

        _playerController.enabled = true;

        if (_currentInteractionCamera != null)
            _currentInteractionCamera.Priority = 0;

        _playerCamera.Priority = 10;
    }
}