using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class BuffUI : MonoBehaviour
    {
        public Image[] buffIcons; // ���� ������ �̹�����
        public Text buffText;     // ���� ���� �� �ؽ�Ʈ
        public int curbuff;       // ���� ���� �� ����
        public Buff buff;

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
            buffText.text = curbuff.ToString();
            Debug.Log("Ȯ��");
        }
    }
}
