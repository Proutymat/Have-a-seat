using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class DialogueLine
{
    public LocalizedString text;

    [Title("Effects")]
    public bool enableShake;
    [Range(0f, 10f), ShowIf("enableShake")] public float shakeIntensity = 0.5f;
}