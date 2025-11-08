using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Backdoor : Interactable
{
    public Transform hinge1;
    public Transform hinge2;
    public Light L;

    public override void onInteract(Transform player)
    {
        DOTween.To(() => L.intensity, x => L.intensity = x, 1f, 1.5f);
        hinge1.DORotate(new(0, -150, 0), 3f).SetEase(Ease.OutBounce);
        hinge2.DORotate(new(0, 150, 0), 3f).SetEase(Ease.OutBounce).OnComplete(() => DOTween.To(() => L.intensity, x => L.intensity = x, 0f, 1.5f));
        Global_Sonar.spawnSonar(transform.position, Vector3.up, 2, 8);
        Destroy(this);
    }
}
