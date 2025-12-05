using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject worldMapUI;
        public GameObject partyFormationUI;
        public MainPlayerControl player;

        public bool isWorldMapOpen = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            // M키로 월드맵 토글
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleWorldMap();
            }

            // ESC로 UI 닫기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseAllUI();
            }
        }

        public void ToggleWorldMap()
        {
            isWorldMapOpen = !isWorldMapOpen;
            worldMapUI.SetActive(isWorldMapOpen);

            player.inputBlocked = isWorldMapOpen; // 전투 입력

            UpdateCursorState();
        }

        public void TogglePartyMenu()
        {
            isWorldMapOpen = !isWorldMapOpen;
            partyFormationUI.SetActive(isWorldMapOpen);

            player.inputBlocked = isWorldMapOpen;

            UpdateCursorState();
        }

        public void OnClickMapBtn()
        {
            ToggleWorldMap();
        }

        public void OnClickPartyMenu()
        {
            TogglePartyMenu();
        }

        public void CloseAllUI()
        {
            // 월드맵만 관리한다면 이렇게
            isWorldMapOpen = false;
            player.inputBlocked = isWorldMapOpen; // 전투 입력
            worldMapUI.SetActive(false);

            UpdateCursorState();
        }

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
