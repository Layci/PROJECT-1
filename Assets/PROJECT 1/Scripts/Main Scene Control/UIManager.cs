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
        public GameObject duplicateWarningPopup; // 팝업창
        public GameObject teleportCheckPopup;
        public MainPlayerControl player;
        public Toggle toggle;
        public Image partyHealMessege;
        public float textDuration;
        public float fadeDuration;
        public bool uiBlock = false;

        // UI 관리 스택
        private Stack<GameObject> uiStack = new Stack<GameObject>();
        private string pendingTeleporterId;

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
                    // 팝업창을 호출해서 볼 수 있도록 설정
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
            // 1. 단축키 입력 (다른 UI가 열려있지 않은 경우에만 UI 호출 가능)
            if (uiStack.Count == 0)
            {
                if (Input.GetKeyDown(KeyCode.F1)) OpenUI(worldMapUI);
                if (Input.GetKeyDown(KeyCode.F2)) OpenUI(partyFormationUI);
                if (Input.GetKeyDown(KeyCode.F3)) OpenUI(settingUI);
            }
            else
            {
                // 현재 열려있는 UI와 단축키가 같다면 닫기(끄기) 기능 수행
                if (Input.GetKeyDown(KeyCode.F1) && uiStack.Peek() == worldMapUI) CloseTopUI();
                if (Input.GetKeyDown(KeyCode.F2) && uiStack.Peek() == partyFormationUI) CloseTopUI();
                if (Input.GetKeyDown(KeyCode.F3) && uiStack.Peek() == settingUI) CloseTopUI();
            }

            // 2. ESC 입력 (가장 최근에 열린 화면부터 닫기)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiStack.Count > 0)
                {
                    CloseTopUI();
                }
            }
        }

        // --- UI 열기 기능 ---
        public void OpenUI(GameObject ui)
        {
            if (uiBlock) return;
            // 이미 다른 UI가 열려있다면 무시 (배타적인 관계)
            if (uiStack.Count > 0 && uiStack.Peek() != ui) return;

            ui.SetActive(true);
            if (!uiStack.Contains(ui))
            {
                uiStack.Push(ui);
            }
            UpdateState();
        }

        // 팝업창 열기 (기존 UI 위에 겹쳐서 표시 할 수 있음)
        public void OpenPopup(GameObject popup)
        {
            // 1. 이미 열려있는 팝업이라면 다시 열지 않음 (중복 방지)
            if (uiStack.Count > 0 && uiStack.Peek() == popup) return;

            popup.SetActive(true);
            uiStack.Push(popup);
            UpdateState();
        }

        // --- UI 닫기 기능 ---
        public void CloseTopUI()
        {
            if (uiStack.Count == 0) return;

            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);

            // 특정 UI를 닫을 때 수행해야 할 추가 처리
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

        // 마우스 커서 및 플레이어 입력 허용 여부 상태 업데이트
        private void UpdateState()
        {
            // ui스택이 하나라도 남아있다면 hasOpenUI는 true가 됨
            bool hasOpenUI = uiStack.Count > 0;
            // ui가 열려있는 동안 플레이어 입력 차단
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

        // 버튼용 Exit 메서드
        public void OnClickExitMap() => CloseTopUI();
        public void OnClickExitParty() => CloseTopUI();
        public void OnClickExitSetting() => CloseTopUI();
        // 텔레포트 취소 버튼 클릭 시
        public void OnClickCancelTeleport()
        {
            pendingTeleporterId = null;
            CloseTopUI();
        }

        // 버튼용 On 메서드
        public void OnClickMap() => OpenUI(worldMapUI);
        public void OnClickParty() => OpenUI(partyFormationUI);
        public void OnClickSetting() => OpenUI(settingUI);
        // 텔레포트 확인 버튼 클릭 시
        public void OnClickConfirmTeleport()
        {
            if (!string.IsNullOrEmpty(pendingTeleporterId))
            {
                WorldMapUI.Instance.TeleportTo(pendingTeleporterId);
                CloseTopUI();
                pendingTeleporterId = null;
            }
        }

        // 텔레포트 확인 팝업 열기
        public void OpenTeleportPopup(string teleporterId)
        {
            pendingTeleporterId = teleporterId;
            OpenPopup(teleportCheckPopup);
        }



        // 중복방지 팝업 처리 (중복방지 활성화)
        public void ConfirmTurnOffDuplicate()
        {
            PartyFormationManager.Instance.preventDuplicate = true;
            PartyFormationManager.Instance.ResetParty();
            PartyFormationWindow.Instance.RefreshUI();
            CloseTopUI(); // 팝업 닫기
        }
        // 중복방지 팝업 처리 (중복방지 해제)
        public void CancelTurnOffDuplicate()
        {
            toggle.isOn = false;
            CloseTopUI(); // 팝업 닫기
        }

        // --------------- 파티원 회복
        IEnumerator ShowHealMessege()
        {
            // 텍스트 기본색상 설정
            partyHealMessege.gameObject.SetActive(true);
            Color originalColor = partyHealMessege.color;
            originalColor.a = 1f;
            partyHealMessege.color = originalColor;

            yield return new WaitForSeconds(textDuration);

            // 페이드 아웃
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                partyHealMessege.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            partyHealMessege.gameObject.SetActive(false);
        }
    }
}
