using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Collider2D swordCollider;
    private Vector2 downAttackOffset = new(0f, -0.05f);
    private Vector2 upAttackOffset = new(-0.02f, 0.12f);
    private Vector2 rightAttackOffset = new(0.1f, 0.05f);
    private Vector2 leftAttackOffset = new(-0.1f, 0.05f);


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

    public void AttackUp()
    {
        transform.localPosition = upAttackOffset;
    }

    public void AttackDown()
    {
        transform.localPosition = downAttackOffset;
    }

    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GoblinController goblin = other.GetComponentInParent<GoblinController>();
            goblin.TakeDamage(0);
        }
    }
}
