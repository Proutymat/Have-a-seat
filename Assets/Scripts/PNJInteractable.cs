using UnityEngine;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private PNJData _pnjData;

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
    }

    public bool CanInteract()
    {
        return !DialogueUI.Instance.IsOpen;
    }
}
