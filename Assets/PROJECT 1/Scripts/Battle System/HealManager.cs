using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Project1;

namespace ProJect1
{
    public class HealManager : MonoBehaviour
    {
        public ParticleSystem healEffect;
        public List<BaseCharacterControl> playerCharacters;
        public static HealManager instance;
        public int healAmount = 30;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            playerCharacters = FindObjectsOfType<BaseCharacterControl>().ToList();
        }

        public void PlayHealEffect()
        {
            if (healEffect != null)
            {
                healEffect.Play();
                HealAllCharacters(); // Èú ½ÇÇà
            }
        }

        private void HealAllCharacters()
        {
            foreach (var character in playerCharacters)
            {
                character.curHealth += healAmount;
                character.curHealth = Mathf.Min(character.curHealth, character.maxHealth);
                Debug.Log($"{character.name} È¸º¹µÊ! ÇöÀç Ã¼·Â: {character.curHealth}");
                character.CheckHP();
            }
        }
    }
}
