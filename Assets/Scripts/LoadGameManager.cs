using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Item Vo_Phong_Kiem, health, herb, speed, wood_sword;
    [SerializeField] GameObject tutorial;
    [SerializeField] HeroController heroController;

    void Start()
    {
        string data = SAVE.LoadFile("Data\\gameData");
        print("data: " + data);
        if (data != "")
        {
            StartCoroutine(PrivateLoadGame(data));
            // PrivateLoadGame(data);
        }
    }

    IEnumerator PrivateLoadGame(string raw_data = "")
    {
        string[] data = raw_data.Split("|");
        string[] items = data[0].Split(",");
        int l = items.Length;
        int health_point = int.Parse(items[l - 1]);
        float posy = float.Parse(items[l - 2]);
        float posx = float.Parse(items[l - 3]);
        int sceneIndex = int.Parse(items[l - 4]);
        l -= 3;
        List<Item> items_list = new();
        foreach (string item in items)
        {
            if (item == "Vo_Phong_Kiem")
                items_list.Add(Vo_Phong_Kiem);
            else if (item == "health")
                items_list.Add(health);
            else if (item == "Cay_Ngu_Trao")
                items_list.Add(herb);
            else if (item == "speed")
                items_list.Add(speed);
            else if (item == "wood sword")
                items_list.Add(wood_sword);
        }
        if (sceneIndex != 0)
        {
            animator.SetTrigger("End");
            yield return new WaitForSeconds(1f);
            tutorial.SetActive(false);
            SceneManager.LoadScene(sceneIndex);

            if (data[1] != ",,")
            {
                string[] re = data[1].Split(",");
                RequestInfo request = new RequestInfo("", 0, Vo_Phong_Kiem);
                if (re[2] == "Vo_Phong_Kiem")
                    request = new RequestInfo(re[0], int.Parse(re[1]), Vo_Phong_Kiem);
                else if (re[2] == "health")
                    request = new RequestInfo(re[0], int.Parse(re[1]), health);
                else if (re[2] == "Cay_Ngu_Trao")
                    request = new RequestInfo(re[0], int.Parse(re[1]), herb);
                else if (re[2] == "speed")
                    request = new RequestInfo(re[0], int.Parse(re[1]), speed);
                else if (re[2] == "wood sword")
                    request = new RequestInfo(re[0], int.Parse(re[1]), wood_sword);

                GameObject requestManagerOB = GameObject.Find("RequestController");
                if (requestManagerOB)
                {
                    RequestController requestManager = requestManagerOB.GetComponent<RequestController>();
                    requestManager.AddNewRequest(request);
                }
                else
                {
                    print("CAN NOT FIND ");
                }
            }
            heroController.Health = health_point;
            print("hero health " + health_point);
            GameObject inventoryManagerOB = GameObject.Find("InventoryManager");
            if (inventoryManagerOB)
            {
                InventoryManager inventoryManager = inventoryManagerOB.GetComponent<InventoryManager>();
                inventoryManager.LoadInventoryFromData(items_list);
                GameObject.Find("RedHero").transform.position = new Vector2(posx, posy);
            }
            else
            {
                print("CAN NOT FIND ");
            }
        }
    }
}
