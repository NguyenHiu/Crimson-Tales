using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] GameObject[] hitboxs;
    string swordName;
    Dir dir = Dir.Down;

    public string SwordName { get { return swordName; } }

    public void SetSwordProperties(int newDamage, string sName)
    {
        swordName = sName;
        foreach (GameObject s in hitboxs)
            s.GetComponent<SwordHitboxController>().SetDamage(newDamage);
    }

    public void Attack(Dir index)
    {
        dir = index;
        hitboxs[(int)dir].GetComponent<SwordHitboxController>().Attack();
    }

    public void EnableSwordCollider()
    {
        print("enable [" + dir + "]");
        hitboxs[(int)dir].GetComponent<SwordHitboxController>().EnableCollider();
    }

    public void DisableSwordCollider()
    {
        print("disable [" + dir + "]");
        hitboxs[(int)dir].GetComponent<SwordHitboxController>().DisableCollider();
    }

    public void StopAttack()
    {
        DisableSwordCollider();
    }
}