using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProJect1.CharacterBase;

namespace ProJect1
{
    public class CharacterBase : MonoBehaviour
    {
        public float currentHP;
        public float maxHP;

        public delegate void OnDamage(float currentHP, float maxHP);
        public delegate void OnCharacterDead();

        public OnDamage onDamageCallbackEvent;
        public OnDamage onDamageCallback;
        public OnCharacterDead onCharacterDead;

        public System.Action<float, float> onDamagedAction;

        public void Damage(float damage)
        {
            currentHP -= damage;

            onDamageCallback(currentHP, maxHP);
            onDamagedAction(currentHP, maxHP);

            if (currentHP <= 0)
            {
                onCharacterDead();
                Destroy(gameObject);
            }
        }

        [ContextMenu("Damage Debug")]
        public void DamageDebugButton()
        {
            Damage(20);
        }
    }
}
