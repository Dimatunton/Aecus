using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Events;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player;

    public int Health = 5;
    public int currentHealth;
    public bool isDead = false;

    public float detectionRange = 10f;
    public float attackRange = 2f;

    public float attackCooldown = 2f;
    public float attackDuration = 1f;

    public float attackDamage = 10f;
    public float attackHitRadius = 1.5f;
    public LayerMask playerLayer;

    public float hitDelay = 0.3f;
    public float detectDuration = 1f;

    public float wanderRadius = 8f;
    public float wanderDelay = 3f;

    // HIT STUN
    public float hitStunDuration = 0.7f;
    private float hitRecoverTime = 0f;

    // 🔊 AUDIO
    public AudioClip idle;
    public AudioClip detect;
    public AudioClip walk;
    public AudioClip attack;
    public AudioClip hurt;
    public AudioClip death;

    // ✅ DAMAGE COOLDOWN (ADDED)
    private float lastDamageTime = -999f;
    public float damageCooldown = 0.1f;

    private float lastAttackTime;
    private NavMeshAgent agent;
    private AudioSource audioSource;

    public Animator animator;

    private string currentState = "";

    private bool isAttacking = false;
    private bool hasDetectedPlayer = false;
    private bool isDetecting = false;
    private bool isHit = false;

    private float wanderTimer;
    private Vector3 homePosition;

    private OutlineController outlineController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        outlineController = GetComponent<OutlineController>();
        audioSource = GetComponent<AudioSource>();

        homePosition = transform.position;
        currentHealth = Health;

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

        wanderTimer = wanderDelay;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;
        if (!agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking || isDetecting || isHit)
        {
            LookAtPlayer();
            return;
        }

        if (Time.time < hitRecoverTime)
        {
            LookAtPlayer();
            return;
        }

        if (!hasDetectedPlayer && distance <= detectionRange)
        {
            StartCoroutine(DetectRoutine());
            return;
        }

        if (distance <= attackRange && hasDetectedPlayer)
        {
            agent.isStopped = true;
            LookAtPlayer();

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else if (distance <= detectionRange && hasDetectedPlayer)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            PlayAnim("Walk");
            PlaySound(walk, true);
        }
        else
        {
            hasDetectedPlayer = false;
            Wander();
        }
    }

    // ---------------- DAMAGE ----------------
    public void hit(int damage)
    {
        if (isDead) return;

        // ✅ 0.1s anti-multi-hit
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        lastDamageTime = Time.time;

        currentHealth -= damage;

        if (hurt != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurt);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (isAttacking)
        {
            StopCoroutine("AttackRoutine");
            isAttacking = false;
        }

        hitRecoverTime = Time.time + hitStunDuration;

        StartCoroutine(HitRoutine());
    }

    // ---------------- REST OF YOUR ORIGINAL CODE ----------------

    void Wander()
    {
        agent.isStopped = false;

        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderDelay)
        {
            Vector3 newPos = RandomNavSphere(homePosition, wanderRadius);
            agent.SetDestination(newPos);
            wanderTimer = 0;
        }

        if (agent.remainingDistance > 0.5f)
        {
            PlayAnim("Walk");
            PlaySound(walk, true);
        }
        else
        {
            PlayAnim("Idle");
            PlaySound(idle, true);
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void PlayAnim(string stateName)
    {
        if (currentState == stateName) return;
        animator.Play(stateName, 0, 0f);
        currentState = stateName;
    }

    void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip == null || audioSource == null) return;

        if (loop)
        {
            if (audioSource.clip == clip && audioSource.isPlaying) return;

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator DetectRoutine()
    {
        isDetecting = true;
        agent.isStopped = true;

        LookAtPlayer();

        PlaySound(detect);
        animator.Play("Detect", 0, 0f);
        currentState = "Detect";

        yield return new WaitForSeconds(detectDuration);

        if (isDead) yield break;

        hasDetectedPlayer = true;
        isDetecting = false;
    }

    IEnumerator AttackRoutine()
    {
        if (isDead || isHit) yield break;

        isAttacking = true;

        outlineController.outlineActivated = true;
        outlineController.outlineLingerDuration = attackDuration;

        agent.isStopped = true;
        LookAtPlayer();

        PlaySound(attack);
        animator.Play("Attack", 0, 0f);
        currentState = "Attack";

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(hitDelay);
        if (isDead || isHit) yield break;

        DetectHit();

        yield return new WaitForSeconds(attackDuration - hitDelay);
        if (isDead || isHit) yield break;

        isAttacking = false;
        PlayAnim("Idle");
    }

    IEnumerator HitRoutine()
    {
        isHit = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        outlineController.outlineActivated = true;
        outlineController.outlineLingerDuration = 1f;

        animator.Play("Hit", 0, 0f);
        currentState = "Hit";

        yield return new WaitForSeconds(0.5f);

        if (isDead) yield break;

        isHit = false;
    }

    void DetectHit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position + transform.forward * attackRange * 0.5f,
            attackHitRadius,
            playerLayer
        );

        foreach (var hit in hitColliders)
        {
            if (hit.transform == player)
            {
                Debug.Log("Player hit!");
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StopAllCoroutines();

        isAttacking = false;
        isDetecting = false;
        isHit = false;

        agent.isStopped = true;
        agent.enabled = false;

        PlaySound(death);
        animator.Play("Death", 0, 0f);
        currentState = "Death";

        
        Destroy(gameObject, 3f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * attackRange * 0.5f,
            attackHitRadius
        );
    }
}