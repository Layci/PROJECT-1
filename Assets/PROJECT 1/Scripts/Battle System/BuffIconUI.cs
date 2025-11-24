using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class BuffIconUI : MonoBehaviour
    {
        public Image[] buffIcons;

        public void UpdateBuffUI(int buffPower)
        {
            for (int i = 0; i < buffIcons.Length; i++)
                buffIcons[i].gameObject.SetActive(i < buffPower);
        }

        /*//public int buffPower = 0; // 버프 파워
        public Image[] buffIcons; // 버프 아이콘 이미지들

        public static BuffIconUI instance;

        private void Awake()
        {
            instance = this;
        }

        public void UpdateBuffUI(int buffPower)
        {
            for (int i = 0; i < buffIcons.Length; i++)
            {
                buffIcons[i].gameObject.SetActive(i < buffPower);
            }
        }

        public void UpdateBuffUI()
        {
            if (buffIcons != null)
            {
                // buffPower 값에 따라 아이콘 활성화 업데이트
                UpdateBuffUI(buffPower);
                Debug.Log("버프 UI 활성화");
            }
        }

        public void IncreaseBuffPower()
        {
            if (buffPower < 3)
                buffPower++;

            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }
        public void DecreaseBuffPower()
        {
            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }*/
    }
}
