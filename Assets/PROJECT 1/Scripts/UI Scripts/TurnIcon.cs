using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnIcon : MonoBehaviour
    {
        public Image iconImage; // 아이콘 이미지
        public Text nameText;   // 캐릭터 이름 텍스트

        public void Setup(Sprite icon, string name)
        {
            iconImage.sprite = icon;
            nameText.text = name;
        }
    }
}
