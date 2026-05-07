using UnityEngine;

public class DialogueShake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _text;
    [SerializeField] private TypewriterEffect _typewriter;

    private Vector3 _originalPos;
    private bool _enabledShake;
    private float _amplitude;
    private float _shakeTimer;

    private void Awake()
    {
        _originalPos = _text.localPosition;
    }

    private void OnEnable()
    {
        TypewriterEffect.CharacterRevealed += OnCharacterRevealed;
    }

    private void OnDisable()
    {
        TypewriterEffect.CharacterRevealed -= OnCharacterRevealed;
    }

    public void SetShakeConfig(bool enabled, float amplitude)
    {
        _enabledShake = enabled;
        _amplitude = amplitude;
        Debug.Log(_enabledShake ? "Enabled" : "Disabled");
    }

    private void OnCharacterRevealed(char c)
    {
        if (!_enabledShake)
            return;

        if (c == ' ' || c == '\n')
            return;

        float multiplier = ".,!?;:".Contains(c) ? 15f : 1f;

        _shakeTimer += _typewriter.CharacterDelay * multiplier;
    }

    private void Update()
    {
        if (!_enabledShake)
            return;

        if (_shakeTimer > 0)
        {
            Debug.Log(_shakeTimer);
            Vector2 offset = Random.insideUnitCircle * _amplitude;

            _text.localPosition = _originalPos + (Vector3)offset;

            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _text.localPosition = Vector3.Lerp(
                _text.localPosition,
                _originalPos,
                Time.deltaTime * 15f
            );
        }
    }
}