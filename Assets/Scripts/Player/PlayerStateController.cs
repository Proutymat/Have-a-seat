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
        State = PlayerState.Sitting;
        _playerController.enabled = false;
        
        // Reset camera pan tilt
        var panTilt = chairCamera.GetComponent<CinemachinePanTilt>();
        panTilt.PanAxis.Value = panTilt.PanAxis.Center;
        panTilt.TiltAxis.Value = panTilt.TiltAxis.Center;

        // Set cameras
        _currentInteractionCamera = chairCamera;
        _playerInteraction.Camera = chairCamera;

        // Set priority
        _currentInteractionCamera.Priority = 10;
        _playerCamera.Priority = 0;
    }

    public void ExitChair(CinemachineCamera chairCamera)
    {
        State = PlayerState.Free;
        _playerController.enabled = true;

        // Set priority
        _currentInteractionCamera.Priority = 0;
        _playerCamera.Priority = 10;
        
        // Set cameras
        _currentInteractionCamera = _playerCamera;
        _playerInteraction.Camera = _playerCamera;
    }

    public void SitOnToilet(CinemachineCamera toiletCamera)
    {
        State = PlayerState.Sitting;
        _playerController.enabled = false;
        
        // Reset camera pan tilt
        var panTilt = toiletCamera.GetComponent<CinemachinePanTilt>();
        panTilt.PanAxis.Value = panTilt.PanAxis.Center;
        panTilt.TiltAxis.Value = panTilt.TiltAxis.Center;

        // Set cameras
        _currentInteractionCamera = toiletCamera;
        _playerInteraction.Camera = toiletCamera;

        // Set priority
        _playerCamera.Priority = 0;
        toiletCamera.Priority = 10;
    }

    public void ExitToilet(CinemachineCamera toiletCamera)
    {
        State = PlayerState.Free;
        _playerController.enabled = true;

        // Set priority
        _playerCamera.Priority = 10;
        _currentInteractionCamera.Priority = 0;

        // Set cameras
        _currentInteractionCamera = _playerCamera;
        _playerInteraction.Camera = _playerCamera;
    }
}