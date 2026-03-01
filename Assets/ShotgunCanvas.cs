using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunCanvas : MonoBehaviour
{
    public Shotgun shotgunScript = null;
    public TextMeshProUGUI bulletText = null;

    private void Start()
    {
        UpdateBullet();
    }
    public void UpdateBullet()
    {
        bulletText.text = shotgunScript.bulletCount.ToString();
    }
}
