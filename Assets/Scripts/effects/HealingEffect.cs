using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : Effect
{
    int heal = 3;

    public override void Affect()
    {
        hero.health += (int)heal;
        if (hero.health > hero.maxHealth) 
            hero.health = hero.maxHealth;
    }

    public override void ClearEffect(){}
    public void SetHeal(int _heal) {
        heal = _heal;
    }
}
