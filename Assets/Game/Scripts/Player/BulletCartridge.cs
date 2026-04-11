using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCartridge : MonoBehaviour
{
    public Shotgun shotgun;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (shotgun.reload())
            {
                Destroy(other.gameObject);
            }
        }
    }
}
