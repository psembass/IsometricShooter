using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] // todo set from code
    private Transform playerTransform;
    private Vector3 lastDestination = Vector3.zero;
    private float destinationThreshold = 0.01f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 playerPos = playerTransform.position;
        // Update destination if player change position more than destinationThreshold
        if (lastDestination == Vector3.zero || (playerPos-lastDestination).magnitude > destinationThreshold)
        {
            agent.SetDestination(playerPos);
        }
    }
}
