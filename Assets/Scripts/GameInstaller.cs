using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private InputActionAsset inputActions;

    public override void InstallBindings()
    {
        Container.Bind<InputActionAsset>().FromInstance(inputActions).AsSingle();
        Container.Bind<IInputHandler>().To<InputHandler>().FromNew().AsSingle();
    }
}
