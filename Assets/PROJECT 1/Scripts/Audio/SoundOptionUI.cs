using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

namespace ProJect1
{
    public class SoundOptionUI : MonoBehaviour
    {
        [Header("Master Settings")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private TMP_InputField masterInput;

        [Header("BGM Settings")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private TMP_InputField bgmInput;

        [Header("SFX Settings")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TMP_InputField sfxInput;

        private void Start()
        {
            if (SoundManager.Instance == null) return;

            // 1. 초기값 세팅
            InitUI(masterSlider, masterInput, SoundManager.Instance.GetMaster());
            InitUI(bgmSlider, bgmInput, SoundManager.Instance.GetBGM());
            InitUI(sfxSlider, sfxInput, SoundManager.Instance.GetSFX());

            // 2. 슬라이더 리스너 연결 (슬라이더 움직일 때 실행)
            masterSlider.onValueChanged.AddListener(SetMaster);
            bgmSlider.onValueChanged.AddListener(SetBGM);
            sfxSlider.onValueChanged.AddListener(SetSFX);

            // 3. 입력창 리스너 연결 (타이핑 중 & 엔터 시 실행)
            masterInput.onValueChanged.AddListener(SetMasterFromInputRealTime);
            bgmInput.onValueChanged.AddListener(SetBGMFromInputRealTime);
            sfxInput.onValueChanged.AddListener(SetSFXFromInputRealTime);

            masterInput.onEndEdit.AddListener(SetMasterFromInputFinal);
            bgmInput.onEndEdit.AddListener(SetBGMFromInputFinal);
            sfxInput.onEndEdit.AddListener(SetSFXFromInputFinal);
        }

        private void InitUI(Slider slider, TMP_InputField input, float value)
        {
            slider.SetValueWithoutNotify(value);
            input.SetTextWithoutNotify(Mathf.RoundToInt(value * 100).ToString());
        }

        // --- 슬라이더 조절 메서드 (숫자창 실시간 동기화 포함) ---
        public void SetMaster(float value)
        {
            SoundManager.Instance.SetVolume("MasterVol", value);
            RefreshInputText(masterSlider, value);
        }

        public void SetBGM(float value)
        {
            SoundManager.Instance.SetVolume("BGMVol", value);
            RefreshInputText(bgmSlider, value);
        }

        public void SetSFX(float value)
        {
            SoundManager.Instance.SetVolume("SFXVol", value);
            RefreshInputText(sfxSlider, value);
        }

        // --- 입력창 조절 로직 ---
        private void UpdateInput(string text, Slider slider, string paramName, bool isFinal)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (float.TryParse(text, out float value))
            {
                // 100 이상의 숫자가 들어오면 즉시 100으로 제한 (Clamp)
                // 0보다 작으면 0, 100보다 크면 100으로 만듭니다.
                float clampedValue = Mathf.Clamp(value, 0f, 100f);

                // 믹서용 0.0 ~ 1.0 값으로 변환
                float normalizedValue = clampedValue / 100f;

                // 슬라이더 위치 업데이트
                slider.SetValueWithoutNotify(normalizedValue);

                // 실제 사운드 볼륨 적용
                SoundManager.Instance.SetVolume(paramName, normalizedValue);

                // 사용자가 100 이상의 값을 입력했거나 입력이 끝났을(Final) 경우
                // 입력창의 글자를 강제로 제한된 숫자로 다시 적용.
                if (isFinal || value > 100)
                {
                    // 사용자가 100이상 타이핑해도 즉시 100으로 글자 수정.
                    RefreshInputText(slider, normalizedValue);
                }
            }
        }

        public void SetMasterFromInputRealTime(string text) => UpdateInput(text, masterSlider, "MasterVol", false);
        public void SetBGMFromInputRealTime(string text) => UpdateInput(text, bgmSlider, "BGMVol", false);
        public void SetSFXFromInputRealTime(string text) => UpdateInput(text, sfxSlider, "SFXVol", false);

        public void SetMasterFromInputFinal(string text) => UpdateInput(text, masterSlider, "MasterVol", true);
        public void SetBGMFromInputFinal(string text) => UpdateInput(text, bgmSlider, "BGMVol", true);
        public void SetSFXFromInputFinal(string text) => UpdateInput(text, sfxSlider, "SFXVol", true);

        // 숫자창 텍스트를 슬라이더 값에 맞춰주는 함수
        private void RefreshInputText(Slider slider, float value)
        {
            // SetTextWithoutNotify를 써서 입력창의 OnValueChanged가 다시 호출되는 무한 루프 방지
            if (slider == masterSlider) masterInput.SetTextWithoutNotify(Mathf.RoundToInt(value * 100).ToString());
            else if (slider == bgmSlider) bgmInput.SetTextWithoutNotify(Mathf.RoundToInt(value * 100).ToString());
            else if (slider == sfxSlider) sfxInput.SetTextWithoutNotify(Mathf.RoundToInt(value * 100).ToString());
        }
    }
}
