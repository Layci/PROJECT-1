using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class BuffUI : MonoBehaviour
    {
        public Image[] buffIcons; // 버프 아이콘 이미지들
        public Text buffText;     // 버프 남은 턴 텍스트
        public int curbuff;       // 버프 남은 턴 정수
        public Buff buff;

        public static BuffUI instance;

        private void Awake()
        {
            // 싱글턴 초기화
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); // 중복된 인스턴스 방지
            }
        }

        private void Start()
        {
            
        }

        public void UpdateBuffUI(int buffPower)
        {
            for (int i = 0; i < buffIcons.Length; i++)
            {
                buffIcons[i].gameObject.SetActive(i < buffPower);
            }   
        }

        public void UpdateBuffTurn(int characterBuff)
        {
            curbuff = characterBuff;
            buffText.text = characterBuff.ToString();
            Debug.Log("버프 턴 UI 업데이트: " + characterBuff);
        }
    }
}
