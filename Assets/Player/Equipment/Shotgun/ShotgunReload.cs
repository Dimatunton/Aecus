using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunReload : MonoBehaviour
{

    public EquipmentSystem equipmentSystem;

    public void addBullet()
    {
        equipmentSystem.addBullet();
    }

}
