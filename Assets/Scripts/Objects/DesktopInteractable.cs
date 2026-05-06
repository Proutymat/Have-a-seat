using Sirenix.OdinInspector;
using UnityEngine;

public class DesktopInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    //[SerializeField] private DesktopSceneController _controller;
    [SerializeField] private PlayerStateController _playerStateController;

    public void Interact()
    {
    }

    public bool CanInteract()
    {
        return true;
    }
}