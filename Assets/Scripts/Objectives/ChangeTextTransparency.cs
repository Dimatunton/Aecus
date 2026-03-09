using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextTransparency : MonoBehaviour
{

    Text text;
    public float alpha = 1.0f;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b,alpha);
    }
}
