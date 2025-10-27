using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ParticlesService : MonoBehaviour
{
    public static string MUZZLE = "MUZZLE";
    public static string BLOOD = "BLOOD";

    private ParticleSystem muzzlePrefab;
    private ParticleSystem bloodPrefab;

    private Dictionary<string, IObjectPool<ParticleSystem>> poolsMap = new();

    [Inject]
    public void Construct(GameConfig gameConfig)
    {
        muzzlePrefab = gameConfig.gunMuzzle;
        bloodPrefab = gameConfig.bloodSplash;
        InitPools();
    }

    private void InitPools()
    {
        InitPool(MUZZLE, muzzlePrefab);
        InitPool(BLOOD, bloodPrefab);
    }

    private void InitPool(string type, ParticleSystem prefab)
    {
        IObjectPool<ParticleSystem> pool = new ObjectPool<ParticleSystem>(
           () => OnCreate(prefab),
           obj => obj.gameObject.SetActive(true),
           obj => obj.gameObject.SetActive(false),
           obj => Destroy(obj),
           false,
           10,
           100
       );
        poolsMap.Add(type, pool);
    }

    private ParticleSystem OnCreate(ParticleSystem prefab)
    {
        ParticleSystem particleSystem = Instantiate(prefab, this.transform);
        particleSystem.gameObject.SetActive(false);
        return particleSystem;
    }

    public void PlayEffect(string effect, Vector3 position)
    {
        poolsMap.TryGetValue(effect, out var pool);
        if (pool != null)
        {
            ParticleSystem particleSystem = pool.Get();
            particleSystem.transform.position = position;
            particleSystem.Play();
            StartCoroutine(ReturnToPoolAfterPlay(particleSystem, effect));
        }
    }

    private IEnumerator ReturnToPoolAfterPlay(ParticleSystem ps, string key)
    {
        yield return new WaitWhile(() => ps.IsAlive(true));
        if (ps != null && poolsMap.ContainsKey(key))
        {
            poolsMap[key].Release(ps);
        }
    }
}
