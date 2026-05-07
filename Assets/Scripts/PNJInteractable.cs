using UnityEngine;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private PNJData _pnjData;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private float _lookHeight = 1f;

    public Vector3 LookPosition => transform.position + Vector3.up * _lookHeight;

    public void Interact()
    {
        GameStage currentStage = GameStateController.Instance.CurrentStage;

        DialogueEntry selectedDialogue = _pnjData.defaultDialogueEntry;

        foreach (DialogueEntry entry in _pnjData.dialogueEntries)
        {
            if (entry != null && entry.Stage == currentStage)
            {
                selectedDialogue = entry;
                break;
            }
        }

        DialogueUI.Instance.Open(selectedDialogue);
        _player.EnterDialogue(LookPosition);
        
    }

    public bool CanInteract()
    {
        return !DialogueUI.Instance.IsOpen;
    }
}
