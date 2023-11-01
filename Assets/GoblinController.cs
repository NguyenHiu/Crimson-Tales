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

public class GoblinController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRender;
    Animator animator;
    TextMeshPro text;

    public ContactFilter2D movementFilter;
    public GoblinSwordController sword;
    private readonly List<RaycastHit2D> castCollisions = new();

    private Vector2 movementInput = Vector2.zero;
    readonly float collisionOffset = .05f;
    readonly float flipSideOffset = .05f;
    bool gettingKnockback = false;
    public int health = 10;
    public int maxHealth = 10;
    private bool canMove = true;

    // AI enemy
    AIPath aIPath;
    AIDestinationSetter aStarDestination;

    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        aIPath = GetComponent<AIPath>();
        aIPath.maxSpeed += Random.Range(1, 11) * .1f;
        aStarDestination = GetComponent<AIDestinationSetter>();
        SetAStarDestination(null);
    }

    void FixedUpdate()
    {
        ShowHealth();

        if (!canMove)
            return;

        // flip X Coordinator of Goblin
        if ((spriteRender.flipX == false &&
             rb.position.x - aStarDestination.target.transform.position.x > flipSideOffset) ||
            (spriteRender.flipX == true &&
             aStarDestination.target.transform.position.x - rb.position.x > flipSideOffset))
        {
            spriteRender.flipX = !spriteRender.flipX;
        }

        /*
            Goblin has 4 states:
                - `Stand`
                - `Move`
                - `Attack`
                - `Hurt`
        */

        // if `Hurt` --> Get Knockback
        if (gettingKnockback == true)
            GetKnockback(aStarDestination.target.transform.position);

        // if do not detect any Player in the zone --> Standing
        else if (!aIPath.canMove)
        {
            animator.SetBool("isWalking", false);
        }

        // if Player in attack zone --> Attack
        else if ((Math.Abs(aStarDestination.target.transform.position.x - rb.position.x) <= 1) &&
                 (Math.Abs(aStarDestination.target.transform.position.y - rb.position.y) <= 0.5) &&
                 aIPath.canMove)
        {
            print("Start Attack");
            animator.SetTrigger("isAttack");
            SwordAttack();
        }

        // else: moving to Player
        else
            animator.SetBool("isWalking", true);
    }

    private void ShowHealth()
    {
        text.text = "HP: " + health;
    }

    private bool TryMove(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
            return false;

        float moveSpeed = 1.5f; //speed;
        // Diagonal moves take longer to finish --> lower speed 
        if (movementInput.x != 0 && movementInput.y != 0)
            moveSpeed *= 0.7071f;

        int count = rb.Cast(
            movementInput,
            movementFilter,
            castCollisions,
            moveSpeed + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * movementInput);
            return true;
        }

        for (int i = 0; i < castCollisions.Count; i++)
        {
            if (//!castCollisions[i].collider.CompareTag("Goblin") &&
                !castCollisions[i].collider.CompareTag("Player"))
            {
                return false;
            }
        }

        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * movementInput);
        return true;
    }

    public void LockMove()
    {
        aIPath.canMove = false;
        aIPath.canSearch = false;
        canMove = false;
    }
    public void UnlockMove()
    {
        print("UnlockMove");
        canMove = true;
        DisableSwordCollider();
    }

    public void EnableSwordCollider()
    {
        sword.swordCollider.enabled = true;
    }

    public void DisableSwordCollider()
    {
        sword.swordCollider.enabled = false;
    }

    public void StopGettingKnockback()
    {
        gettingKnockback = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("isDeath");
            LockMove();
        }
        else
        {
            animator.SetTrigger("isHurt");
            animator.SetBool("isWalking", false);
            gettingKnockback = true;
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void GetKnockback(Vector2 heroPosition)
    {
        aIPath.canMove = false;
        aIPath.canSearch = false;

        movementInput = new Vector2(Random.Range(-1, 2) * 0.8f, Random.Range(-1, 2) * 0.8f);
        if (heroPosition.x > rb.position.x)
            movementInput.x = -0.8f;
        else if (heroPosition.x < rb.position.x)
            movementInput.x = 0.8f;

        if (heroPosition.y > rb.position.y)
            movementInput.y = -0.8f;
        else if (heroPosition.y < rb.position.y)
            movementInput.y = 0.8f;

        TryMove(movementInput);
    }

    public void SwordAttack()
    {
        LockMove();
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

    public void SetHighLayerObject()
    {
        spriteRender.sortingOrder = 1;
    }

    public void SetLowLayerObject()
    {
        spriteRender.sortingOrder = 0;

    }

    public void SetAStarDestination(Transform _transform)
    {
        if (_transform == null)
        {
            aStarDestination.target = target.transform;
            aIPath.canMove = false;
            aIPath.canSearch = false;
        }
        else
        {
            if (canMove && !gettingKnockback)
            {
                aIPath.canMove = true;
                aIPath.canSearch = true;
            }
            aStarDestination.target = _transform;
        }
    }
}
