using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class ButtonManager : MonoBehaviour
    {
        public Sprite x1;
        public Sprite x2;

        public bool isPause = false;
        public bool isFastSpeed = false;

        Image image;
        Button button;

        private void Awake()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
        }

        public void OnClickAttackBtn()
        {
            MeleeCharacterControl.instance.OnClickAttackBtn();
        }

        public void OnClickSkillAttackBtn()
        {
            MeleeCharacterControl.instance.OnClickSkillAttackBtn();
        }

        public void OnClickHighSpeedBtn()
        {
            if (!isPause)
            {
                if(!isFastSpeed)
                {
                    isFastSpeed = true;
                    Time.timeScale = 2.0f;
                    //button.image = x1;
                }
                else
                {
                    isFastSpeed = false;
                    Time.timeScale = 1.0f;
                    image.sprite = x1;
                }
            }
        }
    }
}
