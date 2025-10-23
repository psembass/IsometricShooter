using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner
{
    private Enemy enemyPrefab;
    private int maxEnemies = 10;
    private Transform targetTransform;
    private int enemyCount = 0;

    private IObjectPool<GameObject> enemyPool;
    public event Action OnEnemyKilled;

    public EnemySpawner(Enemy enemyPrefab, GameConfig gameConfig, IPlayer player)
    {
        this.enemyPrefab = enemyPrefab;
        this.maxEnemies = gameConfig.maxEnemies;
        this.targetTransform = player.Transform;
        // Init Enemy pool
        enemyPool = new ObjectPool<GameObject>(
            () => CreateEnemy(),
            obj => OnGetEnemy(obj),
            obj => OnReleaseEnemy(obj),
            obj => GameObject.Destroy(obj),
            false,
            maxEnemies,
            maxEnemies
        );
    }

    private GameObject CreateEnemy()
    {
        GameObject enemyObject = GameObject.Instantiate(enemyPrefab.gameObject);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.SetPlayerTransform(targetTransform);
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
        OnEnemyKilled?.Invoke();
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
}
