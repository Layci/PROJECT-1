using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class SkillPointManager : MonoBehaviour
    {
        public int maxSkillPoint;
        public int curSkillPoint;
        public int curSkillNum;

        public static SkillPointManager instance;

        public Image[] skillPoints;
        public Text curSkillNumText;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            // �����Ҷ� ��ų����Ʈ �̹��� ���� 
            ResetPoint();

            // �̹��� �ʱ�ȭ �ѵ� ���� ��ų����Ʈ ��ŭ �ٽ� ���ֱ�
            for (int i = 0; i < curSkillPoint; i++)
            {
                skillPoints[i].enabled = true;
            }

            curSkillNumText.text = curSkillNum.ToString();
        }

        // ��ų����Ʈ ���
        public void SkillPointUp()
        {
            if (curSkillPoint < maxSkillPoint)
            {
                // ��ų����Ʈ �ʱ�ȭ
                ResetPoint();

                // ��ų����Ʈ ���
                curSkillPoint++;

                curSkillNum++;

                curSkillNumText.text = curSkillNum.ToString();

                // �ٽ� ��ų����Ʈ �̹��� ��Ÿ����
                for (int i = 0; i < curSkillPoint; i++)
                {
                    skillPoints[i].enabled = true;
                }
            }
        }

        // ��ų����Ʈ ����
        public void UseSkillPoint()
        {
            if (curSkillPoint > 0)
            {
                // ��ų����Ʈ �ʱ�ȭ
                ResetPoint();

                // ��ų����Ʈ ����
                curSkillPoint--;

                curSkillNum--;

                curSkillNumText.text = curSkillNum.ToString();

                // �ٽ� ��ų����Ʈ �̹��� ��Ÿ����
                for (int i = 0; i < curSkillPoint; i++)
                {
                    skillPoints[i].enabled = true;
                }
            }
        }

        // ��ų����Ʈ �ʱ�ȭ
        public void ResetPoint()
        {
            for (int i = 0; i < skillPoints.Length; i++)
            {
                skillPoints[i].enabled = false;
            }
        }
    }
}
