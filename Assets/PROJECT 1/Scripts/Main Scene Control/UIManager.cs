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
        public MainPlayerControl player;
        public Toggle toggle;
        public GameObject duplicateWarningPopup;
        public Image partyHealMessege;
        public float textDuration;
        public float fadeDuration;

        public bool isWorldMapOpen = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // 초기 상태 = 켜짐(true)
            toggle.isOn = PartyFormationManager.Instance.preventDuplicate;

            toggle.onValueChanged.AddListener((value) =>
            {
                //PartyFormationManager.Instance.preventDuplicate = value;
                if (value) // ture == 중복 방지 켜기
                {
                    duplicateWarningPopup.SetActive(true);
                }
                else
                {
                    // 단순히 옵션만 켜면 됨
                    PartyFormationManager.Instance.preventDuplicate = value;
                }
                Debug.Log("중복 선택 방지: " + (value ? "켜짐" : "꺼짐"));
            });
        }

        private void Update()
        {
            // M키로 월드맵 토글
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleWorldMap();
            }

            // ESC로 UI 닫기
            if (Input.GetKeyDown(KeyCode.Escape) && !duplicateWarningPopup.activeSelf)
            {
                CloseAllUI();
            }
        }

        // 월드맵을 열었을때
        public void ToggleWorldMap()
        {
            isWorldMapOpen = !isWorldMapOpen;
            worldMapUI.SetActive(isWorldMapOpen);

            player.inputBlocked = isWorldMapOpen; // 전투 입력

            UpdateCursorState();
        }

        // 파티편성 메뉴를 열었을때
        public void TogglePartyMenu()
        {
            isWorldMapOpen = !isWorldMapOpen;
            partyFormationUI.SetActive(isWorldMapOpen);

            player.inputBlocked = isWorldMapOpen;

            UpdateCursorState();
        }

        // 맵 UI 클릭
        public void OnClickMapBtn()
        {
            ToggleWorldMap();
        }

        // 파티편성 UI 클릭
        public void OnClickPartyMenu()
        {
            TogglePartyMenu();
        }

        // 월드맵 닫기
        public void OnCLickExitMap()
        {
            CloseAllUI();
        }

        // 파티편성메뉴 닫기
        public void OnClickExitParty()
        {
            CloseAllUI();
        }

        // UI 닫기
        public void CloseAllUI()
        {
            // 월드맵만 관리한다면 이렇게
            isWorldMapOpen = false;
            player.inputBlocked = isWorldMapOpen; // 전투 입력
            worldMapUI.SetActive(false);
            partyFormationUI.SetActive(false);

            PartyFormationManager.Instance.ReBuildPartyStates();
            UpdateCursorState();
        }

        // 중복방지 허용
        public void ConfirmTurnOffDuplicate()
        {
            // 옵션 활성화 확정
            PartyFormationManager.Instance.preventDuplicate = true;

            // 파티 초기화
            PartyFormationManager.Instance.ResetParty();
            // UI 갱신
            PartyFormationWindow.Instance.RefreshUI();

            // 팝업 닫기
            duplicateWarningPopup.SetActive(false);
        }

        // 중복방지 취소
        public void CancelTurnOffDuplicate()
        {
            // 토글 다시 false로 돌리기 (UI 강제 갱신)
            toggle.isOn = false;

            // 팝업 닫기
            duplicateWarningPopup.SetActive(false);
        }

        IEnumerator ShowHealMessege()
        {
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
                float alpha = Mathf.Lerp(1f, 0, elapsed / fadeDuration);
                partyHealMessege.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            partyHealMessege.gameObject.SetActive(false);
        }

        // 커서 잠금 상태
        private void UpdateCursorState()
        {
            // UI 열려 있으면 마우스 보이고 Unlock
            if (isWorldMapOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("마우스 잠금 해제");
            }
            else  // UI 다 닫히면 Lock
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("마우스 잠금");
            }
        }
    }
}
