using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Collider2D swordCollider;
    // Start is called before the first frame update
    private Vector2 downAttackOffset = new(0f, -0.05f);
    private Vector2 upAttackOffset = new(-0.02f, 0.12f);
    private Vector2 rightAttackOffset = new(0.1f, 0.05f);
    private Vector2 leftAttackOffset = new(-0.1f, 0.05f);


    void Start()
    {
        swordCollider = GetComponent<Collider2D>();
    }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = leftAttackOffset;
    }

    public void AttackUp() {
        swordCollider.enabled = true;
        transform.localPosition = upAttackOffset;
    }

    public void AttackDown() {
        swordCollider.enabled = true;
        transform.localPosition = downAttackOffset;
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }
}
