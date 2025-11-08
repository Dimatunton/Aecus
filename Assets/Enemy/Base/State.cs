using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void onStateStart(Enemy owner);
    public abstract void onStateStay(Enemy owner);
    public abstract void onStateEnd(Enemy owner);
}
