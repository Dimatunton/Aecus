using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected int health = 10;
    [SerializeField] protected int speed = 3;

    public Animator animator;
    public NavMeshAgent agent;

    public State currentState;
    
    

    QuickOutline outline = null;
    Coroutine blinkCoroutine = null;

    public abstract void takeDamage(int damage);


    public virtual void Start()
    {
        tag = "Enemy";
    }
    public virtual void Update()
    {
        if(currentState != null)
        {
            currentState.onStateStay(this);
        }
    }
    public void blinkDetect()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blink());
            blinkCoroutine = null;
        }
        blinkCoroutine = StartCoroutine(blink());
    }
    IEnumerator blink()
    {
        if(outline == null)
        {
            gameObject.TryGetComponent<QuickOutline>(out outline);
            if (outline == null)
            {
                outline = gameObject.AddComponent<QuickOutline>();
            }
            outline.OutlineColor = Color.red;
            outline.OutlineMode = QuickOutline.Mode.OutlineAll;
            
        }
        yield return new WaitForSeconds(1f);

        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 10f, .3f);
        //                                                           end value ^    ^ duration
        yield return new WaitForSeconds(.3f);
        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 0f, .3f);
        yield return new WaitForSeconds(1f);

        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 10f, .3f);
        yield return new WaitForSeconds(.3f);
        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 0f, .3f);
        yield return new WaitForSeconds(1f);

        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 10f, .3f);
        yield return new WaitForSeconds(.3f);
        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 0f, .3f);

        blinkCoroutine = null;
    }
    public virtual void switchState(State nextState)
    {
        if (nextState != currentState)
        {
            currentState = nextState;
        }
    }

}
