using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaneScan : MonoBehaviour
{

    public float lifeSpan = 1f;
    public float maxSize = 1f;

    float timer = 0f;

    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer < lifeSpan)
        {
            transform.localScale += Vector3.one * ((maxSize / lifeSpan) * deltaTime);
        }
        else
        {
            Destroy ( gameObject );
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(!collision.isTrigger && collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.blinkDetect();
        }
    }
}
