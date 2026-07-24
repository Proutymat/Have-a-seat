using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera _camera;

    [Header("Noise Presets")]
    [SerializeField] private NoiseSettings _handheldNoise;
    [SerializeField] private NoiseSettings _shakeNoise;

    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        _noise = _camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetHandheld()
    {
        _noise.NoiseProfile = _handheldNoise;
    }

    public void SetShake()
    {
        _noise.NoiseProfile = _shakeNoise;
    }

    public void SetAmplitude(float amplitude)
    {
        _noise.AmplitudeGain = amplitude;
    }

    public void SetFrequency(float frequency)
    {
        _noise.FrequencyGain = frequency;
    }

    public void StopShake()
    {
        _noise.AmplitudeGain = 0f;
        _noise.FrequencyGain = 0f;
    }
}