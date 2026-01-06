using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ProJect1
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public AudioMixer mixer;
        public AudioMixerGroup masterGroup;
        public AudioMixerGroup bgmGroup;
        public AudioMixerGroup sfxGroup;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // 게임 시작 시 저장된 모든 볼륨 적용
            ApplyAllVolumes();
        }

        public void ApplyAllVolumes()
        {
            ApplyMixerVolume("MasterVol", PlayerPrefs.GetFloat("MasterVol", 1f));
            ApplyMixerVolume("BGMVol", PlayerPrefs.GetFloat("BGMVol", 1f));
            ApplyMixerVolume("SFXVol", PlayerPrefs.GetFloat("SFXVol", 1f));
        }

        // 1. 총괄 Get 메서드
        public float GetVolume(string parameterName)
        {
            // 여기서 기본값(1f)을 일괄적으로 관리할 수 있습니다.
            return PlayerPrefs.GetFloat(parameterName, 1f);
        }

        // 2. 외부 UI에서 부르기 편한 개별 Wrapper 메서드들 (선택 사항이지만 추천)
        public float GetMaster() => GetVolume("MasterVol");
        public float GetBGM() => GetVolume("BGMVol");
        public float GetSFX() => GetVolume("SFXVol");

        // 실제 믹서 조절 + 데이터 저장
        public void SetVolume(string parameterName, float sliderValue)
        {
            ApplyMixerVolume(parameterName, sliderValue);
            // PlayerPrefs 저장은 여기서 한 번만 수행하면 됩니다.
            PlayerPrefs.SetFloat(parameterName, sliderValue);
        }

        private void ApplyMixerVolume(string parameterName, float sliderValue)
        {
            // 선형 보간을 이용한 직관적인 dB 계산
            // $dB = \text{Mathf.Lerp}(-80, 0, \text{sliderValue})$
            float dB = Mathf.Lerp(-80f, 0f, sliderValue);

            if (mixer != null)
            {
                mixer.SetFloat(parameterName, dB);
            }
        }
    }
}
