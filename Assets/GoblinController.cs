using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GoblinController : MonoBehaviour
{
    public GameObject hero;
    Rigidbody2D rb;
    SpriteRenderer spriteRender;
    Animator animator;
    TextMeshPro text;
    public ContactFilter2D movementFilter;
    public GoblinSwordController sword;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Vector2 movementInput = Vector2.zero;
    readonly float speed = .05f;
    readonly float collisionOffset = .05f;
    readonly float flipSideOffset = 1f;
    bool gettingKnockback = false;
    bool canMove = true;
    public int health = 10;
    public int maxHealth = 10;

    Vector2 attackZone = new(1, 1);


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        attackZone = new(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "HP: " + health;
        if (canMove)
        {
            if (gettingKnockback == true)
            {
                GetKnockback();
            }
            else if ((Math.Abs(hero.transform.position.x - transform.position.x) <= attackZone.x) &&
            (transform.position.y - hero.transform.position.y <= attackZone.y) &&
            (transform.position.y - hero.transform.position.y >= 0))
            {
                animator.SetTrigger("isAttack");
                SwordAttack();
                // LockMove();
            }
            else
            {
                movementInput = Vector2.zero;
                if (hero.transform.position.x > transform.position.x)
                {
                    movementInput.x = 1;
                }
                else if (hero.transform.position.x < transform.position.x)
                {
                    movementInput.x = -1;
                }
                if (hero.transform.position.y > (transform.position.y - 0.5))
                {
                    movementInput.y = 1;
                }
                else if (hero.transform.position.y < (transform.position.y - 0.5))
                {
                    movementInput.y = -1;
                }
                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                // side: right
                if ((spriteRender.flipX == false &&
                transform.position.x - hero.transform.position.x > flipSideOffset) ||
                (spriteRender.flipX == true &&
                hero.transform.position.x - transform.position.x > flipSideOffset))
                {
                    spriteRender.flipX = !spriteRender.flipX;
                }

                animator.SetBool("isWalking", success);
            }
        }

    }

    private bool TryMove(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
            return false;
        float moveSpeed = speed;
        if (movementInput.x != 0 && movementInput.y != 0)
        {
            moveSpeed = speed * 0.7071f;
        }

        int count = rb.Cast(
            movementInput,
            movementFilter,
            castCollisions,
            moveSpeed + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed);
            return true;
        }
        bool uCanMove = true;
        for (int i = 0; i < castCollisions.Count; i++)
        {
            if (castCollisions[i].collider.tag != "Goblin")
            {
                uCanMove = false;
                break;
            }
        }
        if (uCanMove)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed);
            return true;
        }
        return false;
    }

    public void LockMove()
    {
        canMove = false;
    }
    public void UnlockMove()
    {
        canMove = true;
        // sword.StopAttack();
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
            canMove = false;
        }
        else
        {
            print("goblin's health: " + health);
            animator.SetTrigger("isHurt");
            animator.SetBool("isWalking", false);
            gettingKnockback = true;
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void GetKnockback()
    {
        movementInput = Vector2.zero;
        if (hero.transform.position.x > transform.position.x)
        {
            movementInput.x = -0.8f;
        }
        else if (hero.transform.position.x < transform.position.x)
        {
            movementInput.x = 0.8f;
        }
        if (hero.transform.position.y > transform.position.y)
        {
            movementInput.y = -0.8f;
        }
        else if (hero.transform.position.y < transform.position.y)
        {
            movementInput.y = 0.8f;
        }
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
}
