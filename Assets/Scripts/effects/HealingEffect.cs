using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : Effect
{
    int heal = 3;

    public override void Affect()
    {
        hero.Healing(heal);
    }

    public override void ClearEffect() { }
    public void SetHeal(int _heal)
    {
        heal = _heal;
    }
}
