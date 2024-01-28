using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    GameObject moveTutorial, chatTutorial, chestTutorial,
    drinkPotionTutorial, attackTutorial, finalText;
    GameObject tutorialGroup;
    [SerializeField] GameObject collider1;
    [SerializeField] GameObject requestSign;
    HeroController heroController;
    InventoryManager inventoryManager;
    GameObject chestInventory;
    [SerializeField] Item speed, health, sword;
    [SerializeField] GameObject goblin;
    [SerializeField] GameObject tinyRocks;

    bool openChest = false, requestSignClose = false, isAttackTutorial = false;

    void Start()
    {
        Canvas mainCanvas = FindAnyObjectByType<Canvas>();
        tutorialGroup = mainCanvas.transform.Find("Tutorials").gameObject;
        moveTutorial = tutorialGroup.transform.Find("Move").gameObject;
        chatTutorial = tutorialGroup.transform.Find("Chat").gameObject;
        chestTutorial = tutorialGroup.transform.Find("Chest").gameObject;
        drinkPotionTutorial = tutorialGroup.transform.Find("Potion").gameObject;
        attackTutorial = tutorialGroup.transform.Find("Attack").gameObject;
        finalText = tutorialGroup.transform.Find("Final").gameObject;
        heroController = FindAnyObjectByType<HeroController>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        chestInventory = mainCanvas.transform.
                                Find("InventoryGroup").
                                Find("ChestInventory").gameObject;

        heroController.Health = 6;
        moveTutorial.SetActive(true);
        chatTutorial.SetActive(false);
        chestTutorial.SetActive(false);
        drinkPotionTutorial.SetActive(false);
        attackTutorial.SetActive(false);
    }

    void FixedUpdate()
    {
        if (heroController && heroController.Health < 5) heroController.Health = 7;

        if (!requestSignClose && requestSign && requestSign.activeInHierarchy == false)
        {
            FromNPCToChest();
            requestSignClose = true;
        }
        if (inventoryManager && inventoryManager.NumberOfItem(health) >= 1)
            drinkPotionTutorial.SetActive(true);
        if (chestInventory && chestInventory.activeInHierarchy)
            FromChestToDragItemTutorial();
        if (openChest && chestInventory && !chestInventory.activeInHierarchy)
        {
            chestTutorial.SetActive(false);
        }
        if (heroController && heroController.Health > 6 && !isAttackTutorial)
        {
            isAttackTutorial = true;
            drinkPotionTutorial.SetActive(false);
            attackTutorial.SetActive(true);
            inventoryManager.AddItem(sword);
            new WaitForSeconds(3f);
            goblin.SetActive(true);
        }
        if (isAttackTutorial && !goblin)
        {
            attackTutorial.SetActive(false);
            heroController.Health = 10;
            finalText.SetActive(true);
        }
        if (tinyRocks && tinyRocks.activeInHierarchy == false)
        {
            tutorialGroup.SetActive(false);
        }
    }

    public void FromMoveToNPC()
    {
        moveTutorial.SetActive(false);
        chatTutorial.SetActive(true);
    }

    public void FromNPCToChest()
    {
        collider1.SetActive(false);
        chatTutorial.SetActive(false);
        chestTutorial.SetActive(true);
    }

    public void FromChestToDragItemTutorial()
    {
        if (openChest) return;
        chestTutorial.SetActive(false);
        openChest = true;
    }

}
