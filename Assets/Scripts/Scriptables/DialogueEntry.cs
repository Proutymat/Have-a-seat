using UnityEngine;

[CreateAssetMenu(menuName = "PNJ/DialogueEntry")]
public class DialogueEntry : ScriptableObject
{
    public GameStage Stage;
    public DialogueLine[] Lines;
}