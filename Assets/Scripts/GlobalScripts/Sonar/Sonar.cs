using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sonar : MonoBehaviour
{
    public float lifeSpan = 1f;
    public float maxSize = 1f;
    public float depth = 1f;

    DecalProjector projector;
    float timer = 0f;
    float halfLifeSpan = 0f;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
        halfLifeSpan = lifeSpan / 2;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer < lifeSpan)
        {
            float newSize = (maxSize / lifeSpan) * deltaTime;
            projector.size += (newSize * Vector3.one);
        }
        else
        {
            Destroy(gameObject);
        }
        if(timer > halfLifeSpan)
        {
            projector.fadeFactor = 1f - ((timer - halfLifeSpan) / halfLifeSpan);
        }

    }
}
