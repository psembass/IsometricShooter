using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthComponent))]
public class Enemy : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;
    private HealthComponent healthComponent;
    private Animator animator;
    private Transform playerTransform;
    private Vector3 lastDestination = Vector3.zero;
    private float destinationThreshold = 0.01f;
    private IWeapon weapon = new MeleeWeapon();
    public event Action<GameObject> OnDeath;
    private float attackRate = 0.5f;
    private float lastAttackTime = 0;
    private bool isAlive = true;
    private IAudioService audioService;
    private AudioConfig audioConfig;

    public void Init(Transform playerTransform, AudioConfig audioConfig, IAudioService audioService)
    {
        this.playerTransform = playerTransform;
        this.audioConfig = audioConfig;
        this.audioService = audioService;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += HandleDeath;
        // Enemy must attack only the player
        weapon.SetOwner(transform);
        weapon.SetTargetMask(LayerMask.GetMask("Player"));
        animator = GetComponentInChildren<Animator>();
        Debug.Log("Animator " + animator);
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Vector3 playerPos = playerTransform.position;
        // Update destination if player change position more than destinationThreshold
        if (lastDestination == Vector3.zero || (playerPos-lastDestination).magnitude > destinationThreshold)
        {
            agent.SetDestination(playerPos);
        }
        animator.SetBool("IsStopped", agent.isStopped);
        // attack if we can
        if (Time.time - lastAttackTime > attackRate)
        {
            if (weapon.Attack(playerPos))
            {
                animator.SetTrigger("Attack");
                audioService.PlayOneShot(audioConfig.Monster_attack, transform.position);
                lastAttackTime = Time.time;
            }
        }
    }

    public void Reset()
    {
        healthComponent.Restore();
        isAlive = true;
    }

    private void HandleDeath() {
        animator.SetTrigger("Death");
        audioService.PlayOneShot(audioConfig.Monster_death, transform.position);
        // to stop from moving
        isAlive = false;
        agent.isStopped = true;
    }

    public void OnDeathAnimationComplete()
    {
        OnDeath?.Invoke(gameObject);
    }

    public void TakeDamage(float damage)
    {
        audioService.PlayOneShot(audioConfig.Monster_damage, transform.position);
        healthComponent.TakeDamage(damage);
    }

    public void OnFootstep()
    {
        audioService.PlayOneShot(audioConfig.Monster_footstep, transform.position);
    }
}
