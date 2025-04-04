using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class BuffTurnUI : MonoBehaviour
    {
        
        public Text buffText;     // ���� ���� �� �ؽ�Ʈ
        public int curbuff;       // ���� ���� �� ����

        public static BuffTurnUI instance;

        private void Awake()
        {
            // �̱��� �ʱ�ȭ
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            
        }

        

        public void UpdateBuffTurn(int characterBuff)
        {
            curbuff = characterBuff;
            buffText.text = characterBuff.ToString();
            Debug.Log("���� �� UI ������Ʈ: " + characterBuff);
        }
    }
}
