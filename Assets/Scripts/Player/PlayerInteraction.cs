using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private float _range = 2f;
    
    [Title("References")]
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private CanvasGroup _interactTextCanvasGroup;

    private IInteractable _current;
    
    public CinemachineCamera Camera { set => _camera = value; }

    private void OnEnable()
    {
        _interactAction.action.performed += Interact;
    }

    private void OnDisable()
    {
        _interactAction.action.performed -= Interact;
    }

    private void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        _current = null;

        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable.CanInteract())
            {
                _current = interactable;
                Debug.Log("Interacting with " + interactable.GetType().Name);
            }
        }

        _interactTextCanvasGroup.alpha = _current != null ? 1f : 0f;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        _current?.Interact();
    }
}