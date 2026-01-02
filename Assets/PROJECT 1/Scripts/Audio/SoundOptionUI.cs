using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace ProJect1
{
    public class SoundOptionUI : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            masterSlider.SetValueWithoutNotify(SoundManager.Instance.GetMaster());
            bgmSlider.SetValueWithoutNotify(SoundManager.Instance.GetBGM());
            sfxSlider.SetValueWithoutNotify(SoundManager.Instance.GetSFX());

            masterSlider.onValueChanged.AddListener(SoundManager.Instance.SetMaster);
            bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGM);
            sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFX);
        }

        // 슬라이더를 통해 호출될 메서드들
        public void SetMasterVolume(float sliderValue)
        {
            UpdateMixerVolume("MasterVol", sliderValue);
        }

        public void SetBGMVolume(float sliderValue)
        {
            UpdateMixerVolume("BGMVol", sliderValue);
        }

        public void SetSFXVolume(float sliderValue)
        {
            Debug.Log($"[SFX] slider = {sliderValue}");
            UpdateMixerVolume("SFXVol", sliderValue);
        }

        // 공통 볼륨 조절 로직
        private void UpdateMixerVolume(string parameterName, float sliderValue)
        {
            // 슬라이더 0일 때 완전 무음(-80dB), 그 외에는 로그 계산
            float dB = sliderValue > 0.0001f ? Mathf.Log10(sliderValue) * 20 : -80f;

            SoundManager.Instance.mixer.SetFloat(parameterName, dB);
            PlayerPrefs.SetFloat(parameterName, sliderValue);
        }

        public void LoadVolumes()
        {
            float master = PlayerPrefs.GetFloat("MasterVol", 1f);
            float bgm = PlayerPrefs.GetFloat("BGMVol", 1f);
            float sfx = PlayerPrefs.GetFloat("SFXVol", 1f);

            masterSlider.SetValueWithoutNotify(master);
            bgmSlider.SetValueWithoutNotify(bgm);
            sfxSlider.SetValueWithoutNotify(sfx);

            SetMasterVolume(master);
            SetBGMVolume(bgm);
            SetSFXVolume(sfx);
        }
    }
}
