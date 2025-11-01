using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private GameConfig _config;
    [SerializeField] 
    private AudioConfig audioConfig;
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private UIManager uIManager;
    [SerializeField]
    private GameObject levelPlane;
    [SerializeField]
    private ParticlesService particlesService;

    public override void InstallBindings()
    {
        // Input
        Container.Bind<InputActionAsset>().FromInstance(inputActions).AsSingle();
        Container.Bind<IInputHandler>().To<InputHandler>().FromNew().AsSingle();
        // Configs
        Container.BindInterfacesAndSelfTo<GameConfig>().FromInstance(_config).AsSingle();
        Container.BindInterfacesAndSelfTo<AudioConfig>().FromInstance(audioConfig).AsSingle();
        // Audio
        Container.Bind<IAudioService>().To<FModAudioService>().FromNew().AsSingle();
        // UI
        Container.BindInterfacesAndSelfTo<UIManager>().FromInstance(uIManager).AsSingle();
        // GameObjects
        Container.BindInterfacesAndSelfTo<ParticlesService>().FromInstance(particlesService).AsSingle();
        Container.Bind<HitscanWeapon>().FromNew().AsSingle();
        Container.Bind<GameObject>().WithId("LevelPlane").FromInstance(levelPlane);
        Container.BindInterfacesAndSelfTo<Enemy>().FromInstance(enemyPrefab).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(player).AsSingle(); // todo create dynamically from prefab
        Container.BindInterfacesAndSelfTo<EnemySpawner>().FromNew().AsSingle();
    }
}
