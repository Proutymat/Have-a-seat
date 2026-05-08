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

    private float _poopGauge;
    private bool _isPooping;
    private float _wipeTimer;

    public void StartPooping()
    {
        _poopGauge = 10;
        _wipeTimer = 5;
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
    
    
    private void Update()
    {
        // POOPING
        if (_player.State < PlayerState.Pooping) return;

        HandlePoopAction();

        if (!_isPooping) return;

        _poopGauge -= Time.deltaTime;
        
        Debug.Log("poop gauge = " + _poopGauge);
        
        // Finished pooping
        if (_poopGauge <= 0 && _player.State == PlayerState.Pooping)
        {
            _player.State = PlayerState.Wipping;
            Debug.Log("PlayerState = "  + _player.State);
            Debug.Log("ToiletState = "  + _scene.State);
        }
        
        
        // WIPING
        if (_player.State != PlayerState.Wipping) return;

        _wipeTimer -= Time.deltaTime;
        
        Debug.Log("wipe timer = " + _wipeTimer);
        
        // Finish wiping
        if (_wipeTimer <= 0)
        {
            _player.ExitToilet();
        }     
    }
}
