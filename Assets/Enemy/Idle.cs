using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public override void onStateStart(Enemy owner)
    {
        throw new System.NotImplementedException();
    }
    public override void onStateStay(Enemy owner)
    {
        owner.animator.Play("Idle");
        print("asdad");
        owner.switchState((owner as SherkWazawski).idle);
    }

    public override void onStateEnd(Enemy owner)
    {
        throw new System.NotImplementedException();
    }
}
