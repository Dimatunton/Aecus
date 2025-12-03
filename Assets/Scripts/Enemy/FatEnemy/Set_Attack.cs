using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Attack : MonoBehaviour
{
    public SimpleEnemy E;

    public void setAttack(int isAttacking)
    {
        if (isAttacking <= 0)
        {
            E.attacking = false;
        }
        else
        {
            E.attacking = true;
        }
    }
}
