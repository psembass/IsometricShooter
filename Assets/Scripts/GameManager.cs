using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnemySpawner))]
public class GameManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    [SerializeField]
    private float enemySpawnRate = 2f;
    private float lastSpawnTime = 0;
    [SerializeField]
    private GameObject levelPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime > enemySpawnRate && enemySpawner.CanSpawnEnemy())
        {
            Vector3 spawnPoint = getEnemySpawnPoint();
            enemySpawner.SpawnEnemy(spawnPoint);
            lastSpawnTime = Time.time;
        }
    }

    private Vector3 getEnemySpawnPoint()
    {
        Bounds bounds = levelPlane.GetComponent<Renderer>().bounds;
        int edge = Random.Range(0, 4);
        return edge switch
        {
            0 => new Vector3(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y, bounds.max.z), // Top
            1 => new Vector3(bounds.max.x, bounds.min.y, Random.Range(bounds.min.z, bounds.max.z)), // Right
            2 => new Vector3(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y, bounds.min.z), // Bottom
            3 => new Vector3(bounds.min.x, bounds.min.y, Random.Range(bounds.min.z, bounds.max.z)), // Left
            _ => bounds.center
        };
    }
}
