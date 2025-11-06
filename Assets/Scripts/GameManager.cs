using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private float enemySpawnRate = 2f;
    private float lastSpawnTime = 0;
    private GameObject levelPlane;
    private PlayerController playerController;
    private UIManager uiManager;
    private AudioConfig audioConfig;
    private IAudioService audioService;
    private IInputHandler inputHandler;

    private int enemyKilled = 0;
    private bool isPaused = false;

    [Inject]
    void Construct(GameConfig gameConfig, 
                   EnemySpawner enemySpawner, 
                   PlayerController player, 
                   UIManager uiManager, 
                   [Inject(Id = "LevelPlane")] GameObject levelPlane, 
                   AudioConfig audioConfig, 
                   IAudioService audioService, 
                   IInputHandler inputHandler)
    {
        this.enemySpawner = enemySpawner;
        this.playerController = player;
        this.uiManager = uiManager;
        this.enemySpawnRate = gameConfig.enemySpawnRate;
        this.levelPlane = levelPlane;
        this.audioConfig = audioConfig;
        this.audioService = audioService;
        this.inputHandler = inputHandler;
    }

    private void Start()
    {
        // Subscribe to game events
        playerController.OnDeath += GameOver;
        UIEvents.OnRestart += Restart;
        UIEvents.OnPause += Pause;
        enemySpawner.OnEnemyKilled += HandleEnemyKilled;
        uiManager.UpdateKillCount(enemyKilled);
        audioService.PlaySoundEvent(audioConfig.Main_theme);
        StartCoroutine(HideControlHints());
        // todo Testing
        enemySpawner.SpawnEnemy(new Vector3(0, 0, 5));
        lastSpawnTime = Time.time;
    }

    private IEnumerator HideControlHints()
    {
        while (!inputHandler.isFiring() && inputHandler.MoveAxis() == Vector2.zero)
        {
            yield return null;
        }
        uiManager.HideControlsHint();
    }

    private void HandleEnemyKilled()
    {
        enemyKilled++;
        uiManager.UpdateKillCount(enemyKilled);
    }

    private void GameOver()
    {
        // stop the game
        Time.timeScale = 0f;
        // stop player input
        playerController.IsPaused = true;
        uiManager.ShowGameOverMenu();
    }
    private void Restart()
    {
        // todo reload enemies and player state etc
        audioService.StopSoundEvent(audioConfig.Main_theme);
        enemyKilled = 0;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        playerController.IsPaused = false;
    }

    private void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            playerController.IsPaused = true;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            playerController.IsPaused = false;
            isPaused = false;
        }
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
