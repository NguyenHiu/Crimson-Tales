using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    public int maxHealth, health;
    public float speed = .08f;
    public float normalSpeed = .08f;
    public float collisionOffset = .05f;
    public SwordController sword;
    public Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer spriteRender;
    public ContactFilter2D movementFilter;
    private readonly List<RaycastHit2D> castCollisions = new();
    private bool canMove = true;
    private int idleSide;
    Vector2 movementInput;

    // inventory
    private bool openInventory = false;
    private bool openChestInventory = false;
    public Image toolbarCover;
    public GameObject inventoryGroup;
    public GameObject playerInventory;
    public GameObject chestInventory;
    public InventoryManager inventoryManager;
    public Vector2 playerInventoryPos = new(0, 100);
    public Vector2 playerInventoryInChestPos = new(400, 155);

    // chest
    public Transform openChest = null;

    // effects
    public List<Effect> effects = new();

    // using potion
    public float timeHold;
    public readonly float timeToUsePotion = 1f;
    public GameObject healthEffectPrefab;
    public GameObject speedEffectPrefab;

    public Item demoItem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        canMove = true;
        timeHold = 0f;
    }

    void Update()
    {
        InventoryControl();
        HandControl();
        Move();
    }

    private void InventoryControl()
    {
        if (Input.GetKeyDown(KeyCode.A) && openInventory == false)
        {
            if (openChestInventory == false)
            {
                bool canFindAChest = GetItemsInNearestChest();
                if (canFindAChest)
                {
                    playerInventory.GetComponent<RectTransform>().localPosition = playerInventoryInChestPos;
                    inventoryGroup.SetActive(true);
                    chestInventory.SetActive(true);
                    toolbarCover.enabled = false;
                    openChestInventory = true;
                    animator.SetBool("isRunning", false);
                    LockMove();
                }
            }
            else
            {
                ReturnItemsToTheChest();
                toolbarCover.enabled = true;
                inventoryGroup.SetActive(false);
                openChestInventory = false;
                UnlockMove();
            }
        }

        else if (Input.GetKeyDown(KeyCode.E) && openChestInventory == false)
        {
            if (openInventory == false)
            {
                playerInventory.GetComponent<RectTransform>().localPosition = playerInventoryPos;
                inventoryGroup.SetActive(true);
                chestInventory.SetActive(false);
                toolbarCover.enabled = false;
                openInventory = true;
                animator.SetBool("isRunning", false);
            }
            else
            {
                toolbarCover.enabled = true;
                inventoryGroup.SetActive(false);
                openInventory = false;
            }
        }
    }

    public bool GetItemsInNearestChest()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .9f);
        foreach (Collider2D obj in colliders)
        {
            if (obj.CompareTag("Chest"))
            {
                openChest = obj.GetComponent<Transform>();
                Transform inventoryTransform = openChest.GetChild(0);
                inventoryTransform.SetParent(chestInventory.transform);
                inventoryTransform.localPosition = new(0, 0);
                inventoryTransform.localScale = new(1, 1, 1);
                return true;
            }
        }
        return false;
    }

    public void ReturnItemsToTheChest()
    {
        if (openChest != null)
        {
            chestInventory.transform.GetChild(1).SetParent(openChest);
            openChest = null;
        }
    }

    private void HandControl()
    {
        if (openInventory == true || openChestInventory == true)
            return;

        Item selectedItem = inventoryManager.GetSelectedItem(false);
        if (selectedItem == null)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && (selectedItem.itemType == ItemType.Sword))
        {
            timeHold = 0;
            sword.SetDamage(selectedItem.GetDamage());
            SwordAttack();
        }

        else if ((selectedItem.itemType == ItemType.Potion) && Input.GetKey(KeyCode.Mouse0))
        {
            if (timeHold == 0f)
                speed = normalSpeed / 2;

            timeHold += Time.deltaTime;

            if (timeHold >= timeToUsePotion)
            {
                timeHold = 0;
                speed = normalSpeed;
                Item potion = inventoryManager.GetSelectedItem(true);
                if (potion.GetPotionType() == PotionType.Health)
                {
                    GameObject healEffectObject = Instantiate(healthEffectPrefab, transform);
                    HealingEffect healEffect = healEffectObject.GetComponent<HealingEffect>();
                    healEffect.SetUpEffect(1, gameObject.GetComponent<HeroController>());
                    healEffect.SetHeal(3);
                    healEffect.Affect();
                }
                else if (potion.GetPotionType() == PotionType.Speed)
                {
                    GameObject speedEffectObject = Instantiate(speedEffectPrefab, transform);
                    SpeedEffect speedEffect = speedEffectObject.GetComponent<SpeedEffect>();
                    speedEffect.SetUpEffect(20f, gameObject.GetComponent<HeroController>());
                    speedEffect.SetSpeed(.02f);
                    speedEffect.Affect();
                }
            }
        }

        else if (timeHold > 0)
        {
            speed = normalSpeed;
            timeHold = 0;
        }

    }

    private void Move()
    {
        if (canMove == false)
            return;
        movementInput = Vector2.zero;
        if (Input.GetKey(KeyCode.C))
        {
            movementInput.x = 1;
            animator.SetInteger("idleIndex", 2);
            idleSide = 2;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            movementInput.x = -1;
            animator.SetInteger("idleIndex", 1);
            idleSide = 1;
        }

        if (Input.GetKey(KeyCode.X))
        {
            movementInput.y = -1;
            animator.SetInteger("idleIndex", 0);
            idleSide = 0;
        }
        else if (Input.GetKey(KeyCode.S))
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
            if (castCollisions[i].collider.CompareTag("Enemy"))
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
        print("lock move");
    }

    public void UnlockMove()
    {
        canMove = true;
        sword.StopAttack();
        print("unlock move");
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
