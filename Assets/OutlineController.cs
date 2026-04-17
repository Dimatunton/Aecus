using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public bool outlineActivated = false;
    public float outlineLingerDuration = 3f;

    Renderer[] renderers;

    float timer = 0f;
    bool isShowingOutline = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if(outlineActivated && !isShowingOutline)
        {
            OutlineOn();
        }
        else if (!outlineActivated && isShowingOutline)
        {
            OutlineOff();
        }

        if (outlineActivated && timer < outlineLingerDuration)
        {
            timer += Time.deltaTime;
        }
        else if (outlineActivated && timer >= outlineLingerDuration)
        {
            OutlineOff();
        }
        else if (!outlineActivated && isShowingOutline)
        {
            OutlineOff();
        }

    }

    public void OutlineOn()
    {
        isShowingOutline = true;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetFloat("_UnlitToggle", 1f);
            }
        }
    }

    public void OutlineOff()
    {
        isShowingOutline = false;
        timer = 0f;
        outlineActivated = false;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetFloat("_UnlitToggle", 0f);
            }
        }
    }
}
