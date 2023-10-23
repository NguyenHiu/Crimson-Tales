using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float speed = .1f;
    public float collisionOffset = .02f;
    public SwordController sword;
    public Rigidbody2D rb;
    public Animator animator;
    public ContactFilter2D movementFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private bool canMove = true;
    private int idleSide;
    Vector2 movementInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            movementInput = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                print("attack");
                SwordAttack();
                // return;
            }
            // else {
            //     print(canMove);
            // }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                print("right");
                movementInput.x = 1;
                animator.SetInteger("idleIndex", 2);
                idleSide = 2;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                print("left");
                movementInput.x = -1;
                animator.SetInteger("idleIndex", 1);
                idleSide = 1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                print("down");
                movementInput.y = -1;
                animator.SetInteger("idleIndex", 0);
                idleSide = 0;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                print("up");
                movementInput.y = 1;
                animator.SetInteger("idleIndex", 3);
                idleSide = 3;
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
            animator.SetBool("isRunning", success);
        }
    }

    private bool TryMove(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero || !canMove)
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
        else
        {
            print("can not move");
            return false;
        }
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
        print("lock");
        canMove = false;
    }

    public void UnlockMove()
    {
        print("unlock");
        canMove = true;
        sword.StopAttack();
    }
}
