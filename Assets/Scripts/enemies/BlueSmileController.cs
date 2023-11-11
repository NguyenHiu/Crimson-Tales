using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Pathfinding;

public class BlueSmileController : Enemy
{
    private int damage = 5;
    public Collider2D attackZone;

    public override void Attack() { }
    public void FastAttack()
    {
        aIPath.maxSpeed = specificSpeed * 2;
    }
    public void RestoreSpeed()
    {
        aIPath.maxSpeed = specificSpeed;
    }

    public void EnableAttackCollider()
    {
        attackZone.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackZone.enabled = false;
    }
}
