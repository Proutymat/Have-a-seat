using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;
    [SerializeField, Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;
    
    private TMP_Text _textBox;

    // Basic Typewriter Functionality
    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;
    
    public bool IsTyping => !_readyForNewText;
    
    private WaitForSeconds _textboxFullEventDelay;
    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;
    
    public float CharacterDelay => 1f / charactersPerSecond;


    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }
    
    public void Play()
    {
        _readyForNewText = false;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.ForceMeshUpdate();

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if (_currentVisibleCharacterIndex >= lastCharacterIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;
                yield break;
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;

            if ((character == '?' || character == '.' || character == ',' || character == ':' ||
                 character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return _simpleDelay;
            }

            CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }

    public void CompleteInstantly()
    {
        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _currentVisibleCharacterIndex = _textBox.textInfo.characterCount;

        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;

        _readyForNewText = true;

        CompleteTextRevealed?.Invoke();
    }
}
