using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BulletPouch : MonoBehaviour
{
    public GameObject pouch1;
    public GameObject pouch2;
    public XRBaseInteractable bullet;

    public void EnterHover()
    {
        print("Hovered");
        pouch1.SetActive(false);
        pouch2.SetActive(true);
    }
    public void exitHover()
    {
        print("unHovered");
        pouch1.SetActive(true);
        pouch2.SetActive(false);
    }

    public void activate(XRBaseInteractor interactor)
    {
        interactor.StartManualInteraction((IXRSelectInteractable)bullet);
    }
}
