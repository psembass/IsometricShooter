using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int maxEnemies = 10;
    private GameObject player;
    private int enemyCount = 0;

    private IObjectPool<GameObject> enemyPool;

    void Start()
    {
        // Init Enemy pool
        enemyPool = new ObjectPool<GameObject>(
            () => CreateEnemy(),
            obj => OnGetEnemy(obj),
            obj => OnReleaseEnemy(obj),
            obj => Destroy(obj),
            false,
            maxEnemies,
            maxEnemies
        );
    }

    private GameObject CreateEnemy()
    {
        GameObject enemyObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.SetPlayerTransform(player.transform);
        enemy.OnDeath += ReturnToPool;
        return enemyObject;
    }

    private void OnGetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().Reset();
        enemyCount++;
        Debug.Log("Enemy count: "+enemyCount);
    }

    private void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyCount--;
        Debug.Log("Enemy count: " + enemyCount);
    }

    private void ReturnToPool(GameObject enemy)
    {
        enemyPool.Release(enemy);
    }

    public void SpawnEnemy(Vector3 position)
    {
        enemyPool.Get().transform.position = position;
    }

    public bool CanSpawnEnemy()
    {
        return enemyCount < maxEnemies;
    }

    public void SetPlayerController(GameObject player)
    {
        this.player = player;
    }
}
