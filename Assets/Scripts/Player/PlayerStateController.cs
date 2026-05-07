using UnityEngine;
using Unity.Cinemachine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerState State { get; private set; } = PlayerState.Free;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private CinemachinePanTilt _panTilt;
    
    private CinemachineCamera _currentInteractionCamera;
    private Vector3 _dialogueTarget;
    private bool _isInDialogue;
    
    
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

    public void EnterDialogue(Vector3 lookPosition)
    {
        State = PlayerState.Dialogue;
        _playerController.enabled = false;
        _dialogueTarget = lookPosition;
        _playerController.StopMovement();
        _isInDialogue = true;
    }
    
    public void ExitDialogue()
    {
        State = PlayerState.Free;
        _playerController.enabled = true;
        _isInDialogue = false;
    }
    
    private float NormalizeAngle(float angle)
    {
        while (angle > 180f)
            angle -= 360f;

        return angle;
    }
    
    private void Update()
    {
        if (!_isInDialogue)
            return;

        Vector3 direction = _dialogueTarget - _playerCamera.transform.position;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 euler = targetRotation.eulerAngles;

        float targetYaw = NormalizeAngle(euler.y);
        float targetPitch = NormalizeAngle(euler.x);

        _panTilt.PanAxis.Value = Mathf.Lerp(
            _panTilt.PanAxis.Value,
            targetYaw,
            Time.deltaTime * 5f
        );

        _panTilt.TiltAxis.Value = Mathf.Lerp(
            _panTilt.TiltAxis.Value,
            targetPitch,
            Time.deltaTime * 5f
        );
    }
}