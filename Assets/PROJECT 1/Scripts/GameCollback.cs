using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class GameCollback : MonoBehaviour
    {
        public bool isGameOver;

        public CharacterBase playerCharacter;

        private void Start()
        {
            playerCharacter.onCharacterDead += GameOverCheck;
        }

        public void GameOverCheck()
        {
            // To do : Game Over
        }
    }
}
