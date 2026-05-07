using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    private void OnEnable()
    {
        TypewriterEffect.CharacterRevealed += OnCharacterRevealed;
    }

    private void OnDisable()
    {
        TypewriterEffect.CharacterRevealed -= OnCharacterRevealed;
    }

    private void OnCharacterRevealed(char c)
    {
        // ignore espaces
        if (c == ' ' || c == '\n')
            return;

        // MAEL : call FMOD (to do later)
    }
}