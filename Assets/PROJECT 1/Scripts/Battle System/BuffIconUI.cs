using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class BuffIconUI : MonoBehaviour
    {
        public int buffPower = 0; // ���� �Ŀ�
        public Image[] buffIcons; // ���� ������ �̹�����

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
                // buffPower ���� ���� ������ Ȱ��ȭ ������Ʈ
                UpdateBuffUI(buffPower);
                Debug.Log("���� UI Ȱ��ȭ");
            }
        }

        public void IncreaseBuffPower()
        {
            if (buffPower < 3)
                buffPower++;

            UpdateBuffUI();  // ���� �Ŀ� �� ���� �� UI ������Ʈ ȣ��
        }
        public void DecreaseBuffPower()
        {
            UpdateBuffUI();  // ���� �Ŀ� �� ���� �� UI ������Ʈ ȣ��
        }

    }
}
