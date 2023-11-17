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
using UnityEditor.MPE;

public abstract class Enemy : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRender;
    protected Animator animator;
    protected TextMeshPro text;

    private readonly List<RaycastHit2D> castCollisions = new();
    private Vector2 movementInput = Vector2.zero;
    protected bool canMove = true;
    protected bool gettingKnockback = false;
    protected int health = 10;
    protected int maxHealth = 10;

    protected float specificSpeed;
    readonly float collisionOffset = .05f;
    readonly float flipSideOffset = .05f;
    public float baseSpeed = 2;
    public float attackRangeX;
    public float attackRangeY;

    // items drop
    public float dropItemRate = .5f;
    public Item itemDropped;
    public GameObject dropItemPrefab;


    // AI enemy
    protected AIPath aIPath;
    protected AIDestinationSetter aStarDestination;

    protected GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        aIPath = GetComponent<AIPath>();
        specificSpeed = baseSpeed + Random.Range(1, 11) * .1f;
        aIPath.maxSpeed = specificSpeed;
        aStarDestination = GetComponent<AIDestinationSetter>();
        SetAStarDestination(null);
    }

    // Update is called once per frame
    void Update()
    {
        ShowHealth();
        if (!canMove) return;
        Flip();
        Move();
    }

    private void Flip()
    {
        if (aStarDestination.target == null)
            return;

        if ((spriteRender.flipX == false &&
             rb.position.x - aStarDestination.target.transform.position.x > flipSideOffset) ||
            (spriteRender.flipX == true &&
             aStarDestination.target.transform.position.x - rb.position.x > flipSideOffset))
        {
            spriteRender.flipX = !spriteRender.flipX;
        }
    }

    private void Move()
    {
        if (aStarDestination.target == null)
            return;

        if (gettingKnockback == true)
            GetKnockback(aStarDestination.target.transform.position);
        else if (aStarDestination.target.CompareTag("Enemy"))
        { }
        else if ((Math.Abs(aStarDestination.target.transform.position.x - rb.position.x) <= attackRangeX) &&
                 (Math.Abs(aStarDestination.target.transform.position.y - rb.position.y) <= attackRangeY))
        {
            animator.SetTrigger("isAttack");
            Attack();
        }

        SetMoveState();
    }

    private void SetMoveState()
    {
        if (aIPath.canMove)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    private void ShowHealth()
    {
        text.text = "HP: " + health;
    }

    private bool TryMove(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
            return false;

        float moveSpeed = baseSpeed;
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
            if (!castCollisions[i].collider.CompareTag("Enemy") &&
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

    public void DropItem()
    {
        if (Random.Range(0, 11) / 10f > dropItemRate)
            return;
        print("drop item");
        GameObject dropItemGO = Instantiate(dropItemPrefab);
        dropItemGO.transform.SetParent(transform.parent);
        dropItemGO.transform.position = transform.position;
        DropItem dropItem = dropItemGO.GetComponent<DropItem>();
        dropItem.SetItemDropped(itemDropped);
        dropItem.Init();
    }

    public void DestroyEnemy()
    {
        DropItem();
        if (gameObject)
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

    public void SetAStarDestination(Transform _transform)
    {
        if (_transform == null)
        {
            aStarDestination.target = transform; // target.transform;
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
            Transform rielHero = _transform;
            aStarDestination.target = rielHero;
        }
    }

    public abstract void Attack();

}
