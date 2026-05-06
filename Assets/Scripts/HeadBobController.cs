using Sirenix.OdinInspector;
using UnityEngine;

public class HeadbobController : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private float _bobFrequency = 3f;
    [SerializeField] private float _bobAmplitude = 0.05f;
    [SerializeField] private float _runMultiplier = 1.2f;
    [SerializeField] private float _smoothSpeed = 10f;
    
    [Title("References")]
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    

    private float _timer;
    private Vector3 _initialLocalPos;

    private void Start()
    {
        _initialLocalPos = _camera.localPosition;
    }

    private void Update()
    {
        HandleHeadbob();
    }

    private void HandleHeadbob()
    {
        if (!_controller.isGrounded || _controller.velocity.magnitude < 0.1f)
        {
            ResetPosition();
            return;
        }

        float speedFactor = _controller.velocity.magnitude;

        float multiplier = speedFactor > 6f ? _runMultiplier : 1f;
        _timer += Time.deltaTime * _bobFrequency * multiplier;

        _timer += Time.deltaTime * _bobFrequency * speedFactor;

        float bobX = Mathf.Cos(_timer) * _bobAmplitude;
        float bobY = Mathf.Sin(_timer * 2f) * _bobAmplitude;

        Vector3 targetPos = _initialLocalPos + new Vector3(bobX, bobY, 0);

        _camera.localPosition = Vector3.Lerp(
            _camera.localPosition,
            targetPos,
            _smoothSpeed * Time.deltaTime
        );
    }

    private void ResetPosition()
    {
        _timer = 0;

        _camera.localPosition = Vector3.Lerp(
            _camera.localPosition,
            _initialLocalPos,
            _smoothSpeed * Time.deltaTime
        );
    }
}