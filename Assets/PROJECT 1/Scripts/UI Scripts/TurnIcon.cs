using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnIcon : MonoBehaviour
    {
        public Image iconImage; // ������ �̹���
        public Text nameText;   // ĳ���� �̸� �ؽ�Ʈ

        public void Setup(Sprite icon, string name)
        {
            if (iconImage != null)
                iconImage.sprite = icon;

            if (nameText != null)
                nameText.text = name;
        }
    }
}