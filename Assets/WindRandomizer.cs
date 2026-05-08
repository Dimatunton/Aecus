using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRandomizer : MonoBehaviour
{
    public float offset = 3f;
    public AudioSource audioSource;

    float timer = 0f;
    float Interval = 15f;

    private void Start()
    {
        randomize();
    }
    void Update()
    {
        if(timer >= Interval)
        {
            timer = 0f;
            Interval = Random.Range(15, 25);
            randomize();
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    public void randomize()
    {
        float randx = Random.Range(-offset, offset);
        float randz = Random.Range(-offset, offset);

        transform.localPosition = new Vector3(randx, 1, randz);
        audioSource.Play();
    }

}
