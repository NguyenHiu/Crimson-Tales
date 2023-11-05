using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    float timeLimit;
    float time;
    protected HeroController hero;

    private void FixedUpdate() {
        time += Time.deltaTime;
        if (time >= timeLimit) {
            ClearEffect();
            Destroy(gameObject);
        }
    }
    public void SetUpEffect(float _timeLimit, HeroController _hero) {
        timeLimit = _timeLimit;
        hero = _hero;
        time = 0;
    }

    public abstract void ClearEffect();
    public abstract void Affect();
    
}
