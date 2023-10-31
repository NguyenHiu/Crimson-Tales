using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroController : MonoBehaviour
{
    public int maxHealth, health;
    readonly float speed = .1f;
    readonly float collisionOffset = .05f;
    public SwordController sword;
    public Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer spriteRender;
    public ContactFilter2D movementFilter;
    private readonly List<RaycastHit2D> castCollisions = new();
    private bool canMove = true;
    private int idleSide;
    Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove == false)
            return;

        movementInput = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SwordAttack();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementInput.x = 1;
            animator.SetInteger("idleIndex", 2);
            idleSide = 2;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementInput.x = -1;
            animator.SetInteger("idleIndex", 1);
            idleSide = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementInput.y = -1;
            animator.SetInteger("idleIndex", 0);
            idleSide = 0;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movementInput.y = 1;
            animator.SetInteger("idleIndex", 3);
            idleSide = 3;
        }

        bool success = TryMove(movementInput);
        if (!success)
        {
            success = TryMove(new Vector2(movementInput.x, 0));
            if (!success)
                success = TryMove(new Vector2(0, movementInput.y));
        }
        animator.SetBool("isRunning", success);
    }

    private bool TryMove(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero || !canMove)
            return false;

        float moveSpeed = speed;
        // Diagonal moves take longer to finish --> lower speed 
        if (movementInput.x != 0 && movementInput.y != 0)
            moveSpeed = speed * 0.7071f;

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

        int Count = count;
        for (int i = 0; i < count; ++i)
        {
            if (castCollisions[i].collider.CompareTag("Goblin"))
                --Count;
        }

        if (Count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed);
            return true;
        }

        return false;
    }

    public void EnableSwordCollider()
    {
        sword.EnableSwordCollider();
    }

    public void DisableSwordCollider()
    {
        sword.DisableSwordCollider();
    }

    public void SwordAttack()
    {
        LockMove();
        animator.SetTrigger("isAttack");
        switch (idleSide)
        {
            case 0:
                sword.AttackDown();
                break;
            case 1:
                sword.AttackLeft();
                break;
            case 2:
                sword.AttackRight();
                break;
            case 3:
                sword.AttackUp();
                break;
        }
    }

    public void LockMove()
    {
        canMove = false;
    }

    public void UnlockMove()
    {
        canMove = true;
        sword.StopAttack();
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
            UnlockMove();
            animator.SetTrigger("isHurt");
        }
    }

    public void DestroyHero()
    {
        Destroy(gameObject);
    }

    public void SetHighLayerObject()
    {
        spriteRender.sortingOrder = 1;
    }

    public void SetLowLayerObject()
    {
        spriteRender.sortingOrder = 0;
    }
}
