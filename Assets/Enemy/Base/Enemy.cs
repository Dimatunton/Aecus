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
    public int health = 10;

    public float speed = 3;
    public float detectionRange = 10;
    public float wanderRange = 3f;

    protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    public Transform playerTransform = null;
    protected bool playerDetected = false;

    QuickOutline outline = null;
    Coroutine blinkCoroutine = null;

    public abstract void TakeDamage(int damage);

    public virtual void Start()
    {
        health = maxHealth;
        tag = "Enemy";
        GetComponent<SphereCollider>().radius = detectionRange / 2;
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out Movement player)/* CHANGE THIS TO PLAYER STAT NEXT TIME*/)
        {
            if (playerTransform == null)
            {
                playerTransform = player.transform;
            }
            playerDetected = true;
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out Movement player))
        {
            playerDetected = false;
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
}
