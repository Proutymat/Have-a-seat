using UnityEngine;

public class DesktopInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private DesktopSceneController _controller;

    public void Interact()
    {
    }

    public bool CanInteract()
    {
        return true;
    }
}