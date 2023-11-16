using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] GameObject[] hitboxs;
    Dir dir = Dir.Down;

    public void SetDamage(int newDamage)
    {
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