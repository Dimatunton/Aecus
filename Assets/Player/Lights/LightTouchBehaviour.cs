using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LightBehaviourScript : MonoBehaviour
{
    [SerializeField] float finalIntensity = .6f;
    float lifeDuration = 3f;

    Light L;

    void Start()
    {
        L = GetComponent<Light>();

        DOTween.To(() => L.intensity, x => L.intensity = x, finalIntensity, lifeDuration * .5f)
           .OnComplete(() =>
           {
               DOTween.To(() => L.intensity, x => L.intensity = x, 0f, lifeDuration * .5f).OnComplete(() =>
               {
                   Destroy(gameObject);
               });
           });
    }
}
