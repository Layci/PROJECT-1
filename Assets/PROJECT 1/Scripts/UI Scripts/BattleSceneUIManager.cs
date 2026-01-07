using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class BattleSceneUIManager : MonoBehaviour
    {
        public static BattleSceneUIManager Instance;

        public GameObject settingUI;

        private Stack<GameObject> uiStack = new Stack<GameObject>();

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
                    CloseTopUI();
                    return;
                }

                if (uiStack.Count == 0) OpenUI(settingUI);
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && uiStack.Peek() == settingUI) CloseTopUI();
                }
            }
        }

        public void OpenUI(GameObject ui)
        {
            if (uiStack.Count > 0 && uiStack.Peek() != ui) return;

            ui.SetActive(true);
            if (uiStack.Contains(ui))
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

        public void CloseTopUI()
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
    }
}
