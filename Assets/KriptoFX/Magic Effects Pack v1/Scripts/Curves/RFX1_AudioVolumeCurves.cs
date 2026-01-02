using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using ProJect1;

public class RFX1_AudioVolumeCurves : MonoBehaviour
{
    public AnimationCurve AudioCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float GraphTimeMultiplier = 1;
    public bool IsLoop;

    private bool canUpdate;
    private float startTime;
    private AudioSource audioSource;
    private float startVolume;
    [SerializeField] private AudioMixerGroup sfxGroup;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
        audioSource.volume = AudioCurve.Evaluate(0);
    }

    private void OnEnable()
    {
        audioSource.outputAudioMixerGroup = SoundManager.Instance.sfxGroup;

        startTime = Time.time;
        canUpdate = true;
        if (audioSource != null) audioSource.volume = AudioCurve.Evaluate(0);

        var sources = GetComponentsInChildren<AudioSource>(true);
        Debug.Log("AudioSource count: " + sources.Length);

        foreach (var src in sources)
        {
            Debug.Log(
                $"{src.gameObject.name} | " +
                $"Output: {src.outputAudioMixerGroup}"
            );
        }
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = AudioCurve.Evaluate(time / GraphTimeMultiplier) * startVolume;
            audioSource.volume = eval;
        }
        if (time >= GraphTimeMultiplier) {
            if (IsLoop) startTime = Time.time;
            else canUpdate = false;
        }
    }
}