using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Pathfinding;

public class GoblinController : Enemy
{
    public GoblinSwordController sword;

    public override void Attack()
    {
        LockMove();
        audioManager.GoblinSwordSound();
        switch (spriteRender.flipX)
        {
            case true:
                sword.AttackLeft();
                break;
            case false:
                sword.AttackRight();
                break;
        }
    }

    public void EnableSwordCollider()
    {
        sword.swordCollider.enabled = true;
    }

    public void DisableSwordCollider()
    {
        sword.swordCollider.enabled = false;
    }

    public void UnlockMove()
    {
        canMove = true;
        DisableSwordCollider();
    }
}
