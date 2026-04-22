using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BulletPouch : MonoBehaviour
{
    public GameObject pouch1;
    public GameObject pouch2;
    public GameObject bullet;

    public GameObject bulletPrefab;

    public void EnterHover()
    {
        pouch1.SetActive(false);
        pouch2.SetActive(true);
    }
    public void exitHover()
    {
        pouch1.SetActive(true);
        pouch2.SetActive(false);
    }

    public void Unselected()
    {
        Instantiate(bulletPrefab,bullet.transform.position,Quaternion.identity);

        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

}
