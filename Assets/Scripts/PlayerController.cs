using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Title("Settings")]
    [Title("Speed", horizontalLine: false)]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 5f;
    [Title("Acceleration", horizontalLine: false)]
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 15f;
    [Title("Jump and fall", horizontalLine: false)]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _gravity = -12f;
    [SerializeField] private float _initialFallVelocity = -2;
    
    
    [Title("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _sprintAction;
    
    
    private Vector2 _moveInput;
    private float _verticalVelocity;
    private Vector3 _currentVelocity;
    private bool _isGrounded;
    private bool _isRunning;
    
    private void OnEnable()
    {
        _moveAction.action.performed += StoreMovementInput;
        _moveAction.action.canceled += StoreMovementInput;
        _jumpAction.action.performed += Jump;
        _sprintAction.action.performed += Sprint;
        _sprintAction.action.canceled += Sprint;
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= StoreMovementInput;
        _moveAction.action.canceled -= StoreMovementInput;
        _jumpAction.action.performed -= Jump;
        _sprintAction.action.performed -= Sprint;
        _sprintAction.action.canceled -= Sprint;
    }

    private void StoreMovementInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();   
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _verticalVelocity = _jumpForce;
            GamepadVibration.Vibrate(0.2f, 0.3f, 0.1f);
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _isRunning = context.performed;
    }

    private void HandleGravity()
    {
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = _initialFallVelocity;
        }
        
        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * _moveInput.y + right * _moveInput.x;
        move = Vector3.ClampMagnitude(move, 1f);
        float speed = _isRunning ? _runSpeed : _walkSpeed;
        
        Vector3 targetVelocity = move * speed;

        float accel = move.magnitude > 0 ? _acceleration : _deceleration;

        _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, accel * Time.deltaTime);

        Vector3 finalMove = _currentVelocity;
        finalMove.y = _verticalVelocity;

        if ((_characterController.Move(finalMove * Time.deltaTime) & CollisionFlags.Above) != 0)
        {
            _verticalVelocity = _initialFallVelocity;
        }
    }

    private void Update()
    {
        _isGrounded =_characterController.isGrounded;
        HandleGravity();
        HandleMovement();
    }
}