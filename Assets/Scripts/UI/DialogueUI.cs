using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public bool IsOpen => _isOpen;

    [Header("References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TypewriterEffect _typewriter;
    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private DialogueShake _shake;

    private DialogueEntry _currentDialogue;

    private DialogueLine _currentLine;
    private int _currentLineIndex;

    private bool _isOpen;

    private void Awake()
    {
        Instance = this;

        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        _interactAction.action.performed += NextLine;
    }

    private void OnDisable()
    {
        _interactAction.action.performed -= NextLine;
    }

    public void Open(DialogueEntry dialogue)
    {
        if (dialogue == null || dialogue.Lines.Length == 0)
            return;

        _currentDialogue = dialogue;
        _currentLineIndex = 0;

        _isOpen = true;

        _panel.SetActive(true);

        ShowLine();
    }

    private void NextLine(InputAction.CallbackContext ctx)
    {
        if (!_isOpen)
            return;

        // Texte encore en train de s'écrire
        if (_typewriter.IsTyping)
        {
            _typewriter.CompleteInstantly();
            return;
        }

        // Ligne suivante
        _currentLineIndex++;

        if (_currentLineIndex >= _currentDialogue.Lines.Length)
        {
            Close();
            return;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        _currentLine = _currentDialogue.Lines[_currentLineIndex];

        StartCoroutine(LoadLine(_currentLine.text));
    }

    private IEnumerator LoadLine(LocalizedString localizedString)
    {
        var handle = localizedString.GetLocalizedStringAsync();
        yield return handle;

        _dialogueText.text = handle.Result;

        _shake.SetShakeConfig(
            _currentLine.enableShake,
            _currentLine.shakeIntensity
        );

        _typewriter.Play();
    }

    private void Close()
    {
        _isOpen = false;

        _panel.SetActive(false);
    }
}