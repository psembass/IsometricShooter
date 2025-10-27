using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    public float PlayerMovementSpeed = 3f;
    public int maxEnemies = 10;
    public float enemySpawnRate = 2f;
    public ParticleSystem gunMuzzle;
    public ParticleSystem bloodSplash;
}
