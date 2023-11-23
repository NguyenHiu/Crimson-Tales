using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitboxController : MonoBehaviour
{
    Collider2D _collider2D;
    int damage;

    void Start()
    {
        _collider2D = GetComponent<PolygonCollider2D>();
    }

    public void Attack()
    {
        _collider2D.enabled = true;
    }

    public void EnableCollider()
    {
        _collider2D.enabled = true;
    }
    public void DisableCollider()
    {
        _collider2D.enabled = false;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHitbox"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
