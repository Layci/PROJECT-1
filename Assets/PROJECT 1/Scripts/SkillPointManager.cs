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
            // 시작할때 스킬포인트 이미지 끄기 
            ResetPoint();

            // 이미지 초기화 한뒤 현재 스킬포인트 만큼 다시 켜주기
            for (int i = 0; i < curSkillPoint; i++)
            {
                skillPoints[i].enabled = true;
            }

            curSkillNumText.text = curSkillNum.ToString();
        }

        // 스킬포인트 상승
        public void SkillPointUp()
        {
            if (curSkillPoint < maxSkillPoint)
            {
                // 스킬포인트 초기화
                ResetPoint();

                // 스킬포인트 상승
                curSkillPoint++;

                curSkillNum++;

                curSkillNumText.text = curSkillNum.ToString();

                // 다시 스킬포인트 이미지 나타내기
                for (int i = 0; i < curSkillPoint; i++)
                {
                    skillPoints[i].enabled = true;
                }
            }
        }

        // 스킬포인트 차감
        public void UseSkillPoint()
        {
            if (curSkillPoint > 0)
            {
                // 스킬포인트 초기화
                ResetPoint();

                // 스킬포인트 차감
                curSkillPoint--;

                curSkillNum--;

                curSkillNumText.text = curSkillNum.ToString();

                // 다시 스킬포인트 이미지 나타내기
                for (int i = 0; i < curSkillPoint; i++)
                {
                    skillPoints[i].enabled = true;
                }
            }
        }

        // 스킬포인트 초기화
        public void ResetPoint()
        {
            for (int i = 0; i < skillPoints.Length; i++)
            {
                skillPoints[i].enabled = false;
            }
        }
    }
}
