using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    public class BattleSceneUIManager : MonoBehaviour
    {
        public static BattleSceneUIManager Instance;

        public GameObject settingUI;
        public GameObject defeatUIPanel;

        private Stack<GameObject> uiStack = new Stack<GameObject>();

        public bool forcePause = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(uiStack.Count > 0)
                {
                    if (uiStack.Peek() == defeatUIPanel)
                        return; // 패배 UI는 닫지 않음

                    CloseTopUI();
                    return;
                }
                else if (uiStack.Count == 0) OpenUI(settingUI);
            }

            if (uiStack.Count > 0 || forcePause) 
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void OpenUI(GameObject ui)
        {
            if (uiStack.Count > 0 && uiStack.Peek() != ui) return;

            ui.SetActive(true);
            if (!uiStack.Contains(ui))
            {
                uiStack.Push(ui);
            }
        }

        public void OpenPopup(GameObject popup)
        {
            if (uiStack.Count > 0 && uiStack.Peek() == popup) return;

            popup.SetActive(true);
            uiStack.Push(popup);
        }

        public void OpenDefeatUI()
        {
            CloseAllUI();          // 다른 UI 다 닫고
            OpenPopup(defeatUIPanel); // 패배 UI는 팝업 성격
        }

        public void CloseTopUI()
        {
            if (uiStack.Count == 0) return;

            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
        }

        public void CloseDefeatUI()
        {
            if (uiStack.Count == 0) return;

            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
        }

        public void CloseAllUI()
        {
            while (uiStack.Count > 0)
            {
                uiStack.Pop().SetActive(false);
            }
        }

        // ------ 버튼용 메서드
        public void OnClickExitSetting() => CloseTopUI();

        public void OnClickSetting() => OpenUI(settingUI);

        public void OnClickRetry()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("BattleScene");
        }

        public void OnClickExitToMain()
        {
            forcePause = true;
            CloseDefeatUI();
            BattleTransitionManager.Instance.EndBattle();
            //Time.timeScale = 1f;
        }
    }
}
