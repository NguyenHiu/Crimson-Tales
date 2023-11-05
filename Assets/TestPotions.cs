using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPotions : MonoBehaviour
{
    public HeroController hero;
    public GameObject healthEffectPrefab, speedEffectPrefab;

    public void Heal() {
        GameObject healEffectObject = Instantiate(healthEffectPrefab, hero.transform);
        HealingEffect healEffect = healEffectObject.GetComponent<HealingEffect>();
        healEffect.SetUpEffect(1, hero);
        healEffect.SetHeal(3);
        healEffect.Affect();
        print("hero's health: " + hero.health);
        // hero.effects.Add(healEffect);
    }

    public void Speed() {
        GameObject speedEffectObject = Instantiate(speedEffectPrefab, hero.transform);
        SpeedEffect speedEffect = speedEffectObject.GetComponent<SpeedEffect>();
        speedEffect.SetUpEffect(5, hero);
        speedEffect.SetSpeed(.02f);
        speedEffect.Affect();
        print("hero's speed: " + hero.speed);
        // hero.effects.Add(speedEffect);
    }
}
