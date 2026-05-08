using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPoopController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] private ToiletSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private InputActionReference _poopAction;
    [SerializeField] private CanvasGroup _poopText;
    [SerializeField] private float _poopTime = 6f;
    [SerializeField] private float _waitTimeAfterPoop = 3f;

    private bool _isPooping;
    private float _timer;

    public void StartPooping()
    {
        _timer = 0;
        _scene.PaperUsedThisPoop = 0;
    }

    private void HandlePoopAction()
    {
        if (_poopAction.action.WasPressedThisFrame())
        {
            _isPooping = true;
            _poopText.alpha = 0;
        }
        else if (_poopAction.action.WasReleasedThisFrame())
        {
            _isPooping = false;
            _poopText.alpha = 1;
        }
    }

    private void PoopLogic()
    {
        HandlePoopAction();

        Debug.Log("timer = " + _timer);
        
        if (!_isPooping)
        {
            _timer = _timer < 0 ? 0 : _timer - Time.deltaTime * 0.5f;
            return;
        }
        
        _timer +=  Time.deltaTime;
        float completion = _timer / _poopTime;
        GamepadVibration.Vibrate(completion, completion, 0.1f);
        
        // Finished pooping
        if (_timer > _poopTime)
        {
            _player.State = PlayerState.Wipping;
            _timer = 0;
            Debug.Log("PlayerState = "  + _player.State);
            Debug.Log("ToiletState = "  + _scene.State);
        }
    }

    private void WipeLogic()
    {
        _timer += Time.deltaTime;
        
        Debug.Log("timer = " + _timer);
        
        // Finish wiping
        if (_timer > _waitTimeAfterPoop)
        {
            _player.ExitToilet();
        }    
    }
    
    
    private void Update()
    {
        if (_player.State == PlayerState.Pooping)
            PoopLogic();
        else if (_player.State == PlayerState.Wipping)
            WipeLogic();
    }
}
