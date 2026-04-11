using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RefDoorObj : MonoBehaviour
{
    public UnityEvent onKnock;

    public int knockCount = 0;

    Animator animator;
    Coroutine knockCoroutine = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(knockCoroutine == null)
        {
            knockCoroutine = StartCoroutine(knock());
        }
    }

    IEnumerator knock()
    {
        if (enabled)
        {
            knockCount++;

            if (knockCount >= 5)
            {
                animator.Play("Open");
                onKnock.Invoke();
                Destroy(this);
            }
            else if (knockCount < 5)
            {
                animator.Play("Wobble");
            }
        }
        yield return new WaitForSeconds(.1f);
        knockCoroutine = null;
    }
}
