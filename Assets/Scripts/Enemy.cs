using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthComponent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private HealthComponent healthComponent;
    private Transform playerTransform;
    private Vector3 lastDestination = Vector3.zero;
    private float destinationThreshold = 0.01f;
    private IWeapon weapon = new MeleeWeapon();
    public event Action<GameObject> OnDeath;
    private float attackRate = 0.5f;
    private float lastAttackTime = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += HandleDeath;
        // Enemy must attack only the player
        weapon.SetOwner(transform);
        weapon.SetTargetMask(LayerMask.GetMask("Player"));
    }

    void Update()
    {
        Vector3 playerPos = playerTransform.position;
        // Update destination if player change position more than destinationThreshold
        if (lastDestination == Vector3.zero || (playerPos-lastDestination).magnitude > destinationThreshold)
        {
            agent.SetDestination(playerPos);
        }
        // attack if we can
        if (Time.time - lastAttackTime > attackRate)
        {
            weapon.Attack(playerPos);
            lastAttackTime = Time.time;
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void Reset()
    {
        healthComponent.Restore();
    }

    private void HandleDeath() => OnDeath?.Invoke(gameObject);
}
