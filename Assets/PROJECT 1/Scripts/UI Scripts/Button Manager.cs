using Project1;
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
        public Image image;

        public bool isPause = false;
        public bool isFastSpeed = false;

        private void Awake()
        {
            image.sprite = sprites[0];
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

        public void OnClickHighSpeedBtn()
        {
            if (!isPause)
            {
                isFastSpeed = !isFastSpeed;
                if (isFastSpeed)
                {
                    image.sprite = sprites[1];
                    Time.timeScale = 2.0f;
                }
                else
                {
                    image.sprite = sprites[0];
                    Time.timeScale = 1.0f;
                }
            }
        }
    }
}
