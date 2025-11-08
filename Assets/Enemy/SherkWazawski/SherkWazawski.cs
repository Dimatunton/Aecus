using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SherkWazawski : Enemy
{
    [Space(10f)]
    [Header("Custom Variables")]

    public State idle;

    public override void Start()
    {
        base.Start();
        currentState = idle;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void takeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
