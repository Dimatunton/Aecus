using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDuration = 1f;
    public float attackDamage = 10f;
    public float attackHitRadius = 1.5f;
    public LayerMask playerLayer;
    public float hitDelay = 0.3f;

    private float lastAttackTime;
    private NavMeshAgent agent;
    public Animator animator;

    private string currentState = "";
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        agent.enabled = true;

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }
    }

    void Update()
    {
        if (player == null || isAttacking) return;
        if (!agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            transform.LookAt(player);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            PlayAnim("Walk");
        }
        else
        {
            agent.isStopped = true;
            PlayAnim("Idle");
        }
    }

    void PlayAnim(string stateName)
    {
        if (currentState == stateName) return;
        animator.Play(stateName);
        currentState = stateName;
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayAnim("Attack");
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(hitDelay);

        DetectHit();

        yield return new WaitForSeconds(attackDuration - hitDelay);

        isAttacking = false;
    }

    void DetectHit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange * 0.5f, attackHitRadius, playerLayer);
        foreach (var hit in hitColliders)
        {
            if (hit.transform == player)
            {
                Debug.Log("Player hit!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange * 0.5f, attackHitRadius);
    }
}