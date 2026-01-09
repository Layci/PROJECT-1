using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class InugamiPlayerControl : BaseCharacterControl
    {
        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();
            Buff attackBuff = new Buff("공격력 증가", 3, 0.2f, 0, typeof(InugamiPlayerControl));
            AddBuff(attackBuff);
        }
    }
}
