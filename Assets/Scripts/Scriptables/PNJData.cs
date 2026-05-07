using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "PNJ/PNJ_Data")]
public class PNJData : ScriptableObject
{
    public DialogueEntry defaultDialogueEntry; 
    public DialogueEntry[] dialogueEntries;
}
