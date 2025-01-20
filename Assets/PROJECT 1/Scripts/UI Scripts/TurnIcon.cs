using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnIcon : MonoBehaviour
    {
        public Image iconImage; // ������ �̹���
        public Text nameText;   // ĳ���� �̸� �ؽ�Ʈ
        public Color playerColor = Color.blue;
        public Color enemyColor = Color.red;

        public void Setup(Sprite icon, string name, bool player)
        {
            if (iconImage != null)
                iconImage.sprite = icon;

            if (nameText != null)
                nameText.text = name;

            if (player)
            {
                nameText.color = playerColor;
            }
            else
            {
                nameText.color = enemyColor;
            }
        }
    }
}
