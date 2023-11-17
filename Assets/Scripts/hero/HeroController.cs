using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRender;

    // health
    [SerializeField] int maxHealth, health;

    // move
    [SerializeField] float maxSpeed = .2f;
    [SerializeField] float normalSpeed = .08f;
    [SerializeField] float collisionOffset = .05f;
    [SerializeField] SwordController sword;
    [SerializeField] ContactFilter2D movementFilter;
    readonly List<RaycastHit2D> castCollisions = new();
    Vector2 movementInput;
    bool canMove = true;
    Dir idleDir;
    float speed;

    // inventory
    bool openInventory = false;
    bool openChestInventory = false;
    [SerializeField] Image toolbarCover;
    [SerializeField] GameObject inventoryGroup;
    [SerializeField] GameObject playerInventory;
    [SerializeField] GameObject chestInventory;
    [SerializeField] InventoryManager inventoryManager;
    Vector2 playerInventoryPos = new(0, 100);
    Vector2 playerInventoryInChestPos = new(400, 155);

    // chest
    Transform openChest = null;

    // potion
    float timeHold;
    [SerializeField] float timeToUsePotion = 1f;
    [SerializeField] GameObject healthEffectPrefab;
    [SerializeField] GameObject speedEffectPrefab;

    // dialog
    bool dialogOn = false;

    // hurt animation
    float hurtingTime = 1f;
    float hurtingCooldown = 0f;
    float alpha = 1f;

    // death aniamtion
    bool death = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        canMove = true;
        timeHold = 0f;
        speed = normalSpeed;
    }

    void Update()
    {
        if (!dialogOn)
        {
            if (Input.GetKeyDown(KeyCode.F)) Interact();
            InventoryControl();
            HandControl();
        }
    }

    void FixedUpdate()
    {
        ActingHurt();
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
                    animator.SetBool("isRun", false);
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
                animator.SetBool("isRun", false);
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
                obj.GetComponent<ChestManager>().OpenChestSpriteUpdate();
                openChest = obj.GetComponent<Transform>();
                Transform inventoryTransform = openChest.GetChild(0);
                inventoryTransform.SetParent(chestInventory.transform);
                inventoryTransform.GetComponent<RectTransform>().sizeDelta = new(196, 137);
                inventoryTransform.localPosition = new(-28, -68);
                inventoryTransform.localScale = new(4, 4, 0);
                return true;
            }
        }
        return false;
    }

    public void ReturnItemsToTheChest()
    {
        if (openChest != null)
        {
            openChest.GetComponent<ChestManager>().CloseChestSpriteUpdate();
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
            animator.SetInteger("direction", 1);
            idleDir = Dir.Side;
            spriteRender.flipX = false;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            movementInput.x = -1;
            animator.SetInteger("direction", 1);
            idleDir = Dir.Side;
            spriteRender.flipX = true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            movementInput.y = -1;
            animator.SetInteger("direction", 0);
            idleDir = Dir.Down;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementInput.y = 1;
            animator.SetInteger("direction", 2);
            idleDir = Dir.Up;
        }
        bool success = TryMove(movementInput);
        if (!success)
        {
            success = TryMove(new Vector2(movementInput.x, 0));
            if (!success)
                success = TryMove(new Vector2(0, movementInput.y));
        }
        animator.SetBool("isRun", success);
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
            if (castCollisions[i].collider.CompareTag("Enemy"))
                --Count;

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
        animator.SetTrigger("Attack");
        sword.Attack(idleDir);
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
        if (death) return;
        health -= damage;
        if (health <= 0)
        {
            // animator.SetTrigger("isDeath");
            LockMove();
            hurtingCooldown = hurtingTime;
            alpha = 0;
            death = true;
        }
        else
        {
            UnlockMove();
            // animator.SetTrigger("isHurt");
            hurtingCooldown = hurtingTime;
            alpha = 0;
        }
    }

    void ActingHurt()
    {
        if (hurtingCooldown <= 0f)
        {
            if (death)
            {
                DestroyHero();
                return;
            }
            hurtingCooldown = 0f;
            alpha = 1f;
            spriteRender.color = new Vector4(
                spriteRender.color.r,
                spriteRender.color.g,
                spriteRender.color.b,
                alpha
            );
            return;
        }
        alpha = alpha * 2 + Time.deltaTime;
        if (alpha >= 1f) alpha -= 1f;

        spriteRender.color = new Vector4(
            spriteRender.color.r,
            spriteRender.color.b,
            spriteRender.color.g,
            alpha
        );

        hurtingCooldown -= Time.deltaTime;
    }

    public void DestroyHero()
    {
        if (gameObject) Destroy(gameObject);
    }

    public bool ReceiveItem(Item item)
    {
        if (item) return inventoryManager.AddItem(item);
        return false;
    }

    void Interact()
    {
        Vector2 direction = new(1, 0);
        if (idleDir == Dir.Side) direction = new(spriteRender.flipX ? -1 : 1, 0);
        else if (idleDir == Dir.Down) direction = new(0, -1);
        else if (idleDir == Dir.Up) direction = new(0, 1);

        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            .4f);
        foreach (RaycastHit2D obj in castCollisions)
        {
            if (obj.transform.CompareTag("NPC"))
            {
                print("let's interact");
                obj.transform.GetComponent<Interactable>().Interact(this);
            }
        }
    }

    public void SetDialogOn()
    {
        dialogOn = true;
    }

    public void SetDialogOff()
    {
        dialogOn = false;
    }

    public void AddSpeed(float add)
    {
        speed += add;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    public void SlowDown(float s)
    {
        if (speed > s) speed -= s;
        else speed = 0;
    }

    public void Healing(int h)
    {
        health += h;
        if (health > maxHealth) health = maxHealth;
    }

    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
}

public enum Dir
{
    Down = 0,
    Side = 1,
    Up = 2,
}