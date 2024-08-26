using ProJect1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public TurnSystem turnSystem;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>();  // TurnSystem¿ª √£¿Ω
        }
    }
}
