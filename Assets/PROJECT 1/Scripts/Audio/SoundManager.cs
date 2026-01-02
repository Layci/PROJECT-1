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

        public void SetMaster(float value)
        {
            //mixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("MasterVol", value);

            UpdateMixerVolume("MasterVol", value);
        }

        public float GetMaster()
        {
            return PlayerPrefs.GetFloat("MasterVol", 1f);
        }

        public void SetBGM(float value)
        {
            //mixer.SetFloat("BGMVol", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("BGMVol", value);

            UpdateMixerVolume("BGMVol", value);
        }

        public float GetBGM()
        {
            return PlayerPrefs.GetFloat("BGMVol", 1f);
        }

        public void SetSFX(float value)
        {
            //mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("SFXVol", value);

            UpdateMixerVolume("SFXVol", value);
        }

        public float GetSFX()
        {
            return PlayerPrefs.GetFloat("SFXVol", 1f);
        }

        private void UpdateMixerVolume(string parameterName, float sliderValue)
        {
            // 슬라이더 0일 때 완전 무음(-80dB), 그 외에는 로그 계산
            float dB = sliderValue > 0.0001f ? Mathf.Log10(sliderValue) * 20 : -80f;

            mixer.SetFloat(parameterName, dB);
            PlayerPrefs.SetFloat(parameterName, sliderValue);
        }

        /*public AudioMixer masterMixer;
        public AudioMixerGroup sfxGroup;
        public static SoundManager Instance;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            LoadVolumes();
        }

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

            masterMixer.SetFloat(parameterName, dB);
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
        }*/
    }
}
