using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 3f;
    private IInputHandler InputHandler;
    private CharacterController _characterController;
    private HealthComponent _healthComponent;
    private Camera _camera;

    private float MoveThreshold = 0.001f;
    private IWeapon currentWeapon;

    [Inject]
    private void Construct(IInputHandler inputHandler)
    {
        InputHandler = inputHandler;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _healthComponent = GetComponent<HealthComponent>();
        currentWeapon = new HitscanWeapon();
        currentWeapon.SetOwner(transform);
        _healthComponent.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Game over");
        // todo game over logic - send to game manager
        Time.timeScale = 0f; // stop game
    }

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveAxis = InputHandler.MoveAxis();
        if (moveAxis.magnitude > MoveThreshold)
        {
            Vector3 moveVector = _camera.transform.TransformDirection(moveAxis);
            moveVector.y = 0;
            moveVector = moveVector.normalized;
            _characterController.Move(MovementSpeed * Time.deltaTime * moveVector);
        }
        // Rotate character to aim direction
        Vector3 aimDirection = GetAimDirection();
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
        transform.rotation = targetRotation;
        if (InputHandler.isFiring())
        {
            // attack using current weapon
            currentWeapon.Attack(aimDirection);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 aimDirection = GetAimDirection();
        Gizmos.DrawLine(transform.position, transform.position + aimDirection);
    }

    // Returns aim direction normalized
    private Vector3 GetAimDirection()
    {
        Vector2 look = InputHandler.LookAxis();
        Ray ray = _camera.ScreenPointToRay(look);
        Plane plane = new Plane(Vector3.up, transform.position);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            return (point - transform.position).normalized;
        }
        return Vector3.zero;
    }
}
