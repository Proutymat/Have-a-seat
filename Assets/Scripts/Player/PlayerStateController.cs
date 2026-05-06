using UnityEngine;
using Unity.Cinemachine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerState State { get; private set; } = PlayerState.Free;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private CinemachineCamera _playerCamera;

    private CinemachineCamera _currentInteractionCamera;
    
    
    public void EnterChair(CinemachineCamera chairCamera)
    {
        var panTilt = chairCamera.GetComponent<CinemachinePanTilt>();
        panTilt.PanAxis.Value = 0f;
        panTilt.TiltAxis.Value = 0f;
        
        State = PlayerState.Sitting;

        _playerController.enabled = false;

        _currentInteractionCamera = chairCamera;
        _playerInteraction.Camera = chairCamera;

        _playerCamera.Priority = 0;
        chairCamera.Priority = 10;
    }

    public void ExitChair(CinemachineCamera chairCamera)
    {
        State = PlayerState.Free;

        _playerController.enabled = true;

        _currentInteractionCamera = _playerCamera;
        _playerInteraction.Camera = _playerCamera;

        if (_currentInteractionCamera != null)
            _currentInteractionCamera.Priority = 0;

        _playerCamera.Priority = 10;
        chairCamera.Priority = 0;
    }
}