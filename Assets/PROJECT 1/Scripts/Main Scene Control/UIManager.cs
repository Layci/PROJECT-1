using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject worldMapUI;
        public GameObject partyFormationUI;
        public GameObject settingUI;
        public MainPlayerControl player;
        public Toggle toggle;
        public GameObject duplicateWarningPopup; // 팝업창
        public Image partyHealMessege;
        public float textDuration;
        public float fadeDuration;

        // UI 관리용 스택
        private Stack<GameObject> uiStack = new Stack<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            toggle.isOn = PartyFormationManager.Instance.preventDuplicate;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    // 팝업창은 중첩해서 열 수 있도록 설계
                    OpenPopup(duplicateWarningPopup);
                }
                else
                {
                    PartyFormationManager.Instance.preventDuplicate = value;
                }
            });
        }

        private void Update()
        {
            // 1. 단축키 관리 (다른 UI가 열려있지 않을 때만 새 UI 열기 허용)
            if (uiStack.Count == 0)
            {
                if (Input.GetKeyDown(KeyCode.F1)) OpenUI(worldMapUI);
                if (Input.GetKeyDown(KeyCode.F2)) OpenUI(partyFormationUI);
                if (Input.GetKeyDown(KeyCode.F3)) OpenUI(settingUI);
            }
            else
            {
                // 현재 열려있는 UI가 단축키와 같다면 토글(닫기) 기능 제공
                if (Input.GetKeyDown(KeyCode.F1) && uiStack.Peek() == worldMapUI) CloseTopUI();
                if (Input.GetKeyDown(KeyCode.F2) && uiStack.Peek() == partyFormationUI) CloseTopUI();
                if (Input.GetKeyDown(KeyCode.F3) && uiStack.Peek() == settingUI) CloseTopUI();
            }

            // 2. ESC 관리 (가장 최근에 열린 순서대로 닫기)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiStack.Count > 0)
                {
                    CloseTopUI();
                }
            }
        }

        // --- UI 열기 로직 ---
        public void OpenUI(GameObject ui)
        {
            // 이미 다른 UI가 열려있다면 무시 (스타레일 방식)
            if (uiStack.Count > 0 && uiStack.Peek() != ui) return;

            ui.SetActive(true);
            if (!uiStack.Contains(ui))
            {
                uiStack.Push(ui);
            }
            UpdateState();
        }

        // 팝업창 전용 (기존 UI 위에 겹쳐서 열릴 수 있음)
        public void OpenPopup(GameObject popup)
        {
            // 1. 이미 열려있는 팝업이라면 다시 열지 않음 (중복 방지)
            if (uiStack.Count > 0 && uiStack.Peek() == popup) return;

            popup.SetActive(true);
            uiStack.Push(popup);
            UpdateState();
        }

        // --- UI 닫기 로직 ---
        public void CloseTopUI()
        {
            if (uiStack.Count == 0) return;

            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);

            // 특정 UI가 닫힐 때 실행해야 할 로직 처리
            if (topUI == partyFormationUI)
            {
                PartyFormationManager.Instance.RebuildPartyData();
            }

            UpdateState();
        }

        public void CloseAllUI()
        {
            while (uiStack.Count > 0)
            {
                uiStack.Pop().SetActive(false);
            }
            UpdateState();
        }

        // 마우스 커서 및 플레이어 입력 제어 통합 관리
        private void UpdateState()
        {
            // ui스택이 하나라도 열려있을 경우 hasOpenUI를 true로
            bool hasOpenUI = uiStack.Count > 0;
            // ui가 열려있을 경우 플레이어 공격 잠금
            player.inputBlocked = hasOpenUI;

            if (hasOpenUI)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // 버튼용 메서드들
        public void OnClickExitMap() => CloseTopUI();
        public void OnClickExitParty() => CloseTopUI();
        public void OnClickExitSetting() => CloseTopUI();

        // 중복방지 팝업 처리 (중복방지 비허용)
        public void ConfirmTurnOffDuplicate()
        {
            PartyFormationManager.Instance.preventDuplicate = true;
            PartyFormationManager.Instance.ResetParty();
            PartyFormationWindow.Instance.RefreshUI();
            CloseTopUI(); // 팝업 닫기
        }
        // 중복방지 팝업 처리 (중복방지 허용)
        public void CancelTurnOffDuplicate()
        {
            toggle.isOn = false;
            CloseTopUI(); // 팝업 닫기
        }
    }
}
