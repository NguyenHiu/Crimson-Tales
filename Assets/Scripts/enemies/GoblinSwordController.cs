using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GoblinSwordController : MonoBehaviour
{
    public Collider2D swordCollider;
    Vector2 leftAttackOffset = new(-.06f, 0);
    Vector2 rightAttackOffset = Vector2.zero;

    void Start()
    {
        swordCollider = GetComponent<Collider2D>();
    }

    public void AttackRight()
    {
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        transform.localPosition = leftAttackOffset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            HeroController hero = other.GetComponentInParent<HeroController>();
            hero.TakeDamage(4);
        }
    }
}
