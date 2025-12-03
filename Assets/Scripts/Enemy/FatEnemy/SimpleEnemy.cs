using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SimpleEnemy : Enemy
{

    public enum States { IDLE, ATTACK ,GOBACKTOIDLE}
    public States CurrentState = States.IDLE;

    private Vector3 OriginalPosition = Vector3.zero;

    //IDLE STATE OWN VARIABLES
    bool runOnce_IDLE = false;
    Coroutine idleCoroutine_IDLE = null;

    //ATTACK STATE OWN VAIRABLES
    public bool attacking = false;
    public Rig headAimRig;
    public Transform headTarget;

    Coroutine goToPlayerCoroutine_ATTACK = null;

    public override void Start()
    {
        base.Start();
        OriginalPosition = transform.position;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case States.IDLE:
                if (!runOnce_IDLE)
                {
                    animator.CrossFade("Walk",.2f);
                    //Walk animation
                    agent.SetDestination(OriginalPosition + new Vector3(OriginalPosition.x + Random.Range(-wanderRange, wanderRange), 0f, OriginalPosition.x + Random.Range(-wanderRange, wanderRange)));
                    runOnce_IDLE = true;
                }

                if (agent.remainingDistance <= .1f)
                {
                    if(idleCoroutine_IDLE == null)
                    {
                        idleCoroutine_IDLE = StartCoroutine(IdleRandom());
                    }
                }

                if (playerDetected)
                {
                    StopAllCoroutines();
                    runOnce_IDLE = false;
                    CurrentState = States.ATTACK;
                }

                break;
            case States.ATTACK:
                if(health < maxHealth)
                {
                    playerDetected = true;
                }
                if(playerDetected)
                {
                    headAimRig.weight = Mathf.Lerp(headAimRig.weight, 1f, .2f);
                    headTarget.position = playerTransform.position + (Vector3.up * .75f);
                    if((transform.position - playerTransform.position).magnitude > 2f)
                    {
                        if (!attacking && goToPlayerCoroutine_ATTACK == null)
                        {
                            goToPlayerCoroutine_ATTACK = StartCoroutine(Gotoplayer());
                        }
                    }
                    else
                    {
                        if (!attacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .99f)
                        {
                            agent.ResetPath();
                            transform.LookAt(playerTransform.position, Vector3.up);
                            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                            animator.CrossFade("Attack", .3f,0,0f);
                        }
                    }

                }
                else
                {
                    headAimRig.weight = Mathf.Lerp(headAimRig.weight, 0f, .2f);
                    if (!attacking & agent.remainingDistance < .1f)
                    {
                        StopAllCoroutines();
                        goToPlayerCoroutine_ATTACK = null;
                        CurrentState = States.IDLE;
                    }
                }

                break;
        }
    }

    IEnumerator IdleRandom()
    {
        //Idle animation
        animator.CrossFade("Idle", .2f);
        yield return new WaitForSeconds(Random.Range(1f,5f));
        //Walk animation
        animator.CrossFade("Walk", .2f);

        agent.SetDestination(OriginalPosition + new Vector3(OriginalPosition.x + Random.Range(-wanderRange, wanderRange), 0f, OriginalPosition.x + Random.Range(-wanderRange, wanderRange)));
        idleCoroutine_IDLE = null;
    }

    IEnumerator Gotoplayer()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Attack"))
        {
            animator.Play("Walk_Attack");
        }
        agent.SetDestination(playerTransform.position);
        yield return new WaitForSeconds(.1f);
        goToPlayerCoroutine_ATTACK = null;
        
    }
    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
