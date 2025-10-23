using Project1;
using ProJect1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class ButtonManager : MonoBehaviour
    {
        public Sprite[] sprites;
        public RectTransform attackBtn;
        public RectTransform skillBtn;
        public Image image;
        public static ButtonManager instance;

        public bool isPause = false;
        public bool isFastSpeed = false;

        private void Awake()
        {
            image.sprite = sprites[0];
            instance = this;
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
        }

        /*public void OnClickAttackBtn()
        {
            BaseCharacterControl.instance.OnClickAttackBtn();
        }

        public void OnClickSkillAttackBtn()
        {
            BaseCharacterControl.instance.OnClickSkillAttackBtn();
        }*/

        // 공격 버튼 하이라이트
        public void HighlightBtn()
        {
            // 현재 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;

            if (cur is BaseCharacterControl player)
            {
                if (player.prepareState == AttackPrepareState.Basic)
                {
                    attackBtn.sizeDelta = new Vector2(400, 400);
                    skillBtn.sizeDelta = new Vector2(300, 300);
                }
                else if (player.prepareState == AttackPrepareState.Skill)
                {
                    skillBtn.sizeDelta = new Vector2(400, 400);
                    attackBtn.sizeDelta = new Vector2(300, 300);
                }
                else if (player.prepareState == AttackPrepareState.None)
                {
                    attackBtn.sizeDelta = new Vector2(300, 300);
                    skillBtn.sizeDelta = new Vector2(300, 300);
                }

            }
        }

        public void OnClickHighSpeedBtn()
        {
            if (!isPause)
            {
                isFastSpeed = !isFastSpeed;

                // 버튼 이미지 변경
                image.sprite = isFastSpeed ? sprites[1] : sprites[0];

                foreach (var unit in TurnSystem.instance.allCharacters)
                {
                    if (isFastSpeed)
                    {
                        // 배속 ON → 현재 애니메이션 속도 1.5배로
                        unit.animationSpeed *= 1.5f;
                        unit.moveSpeed *= 1.5f;
                    }
                    else
                    {
                        // 배속 OFF → 원래대로 1.5로 나누기
                        unit.animationSpeed /= 1.5f;
                        unit.moveSpeed /= 1.5f;
                    }

                    unit.AnimationSpeedCheck(); // 갱신
                }
            }
        }

        private void ApplyAnimationSpeed(float multiplier)
        {
            foreach (var unit in TurnSystem.instance.allCharacters)
            {
                unit.animationSpeed *= multiplier;
                unit.AnimationSpeedCheck();
            }
        }
    }
}
