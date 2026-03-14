using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleBG : MonoBehaviour
{
    public Text text;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (text.text.Length < 1)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }
}
