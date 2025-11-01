using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour, IPlayer, IDamageable
{
    [SerializeField]
    private GameObject weaponBarrel;
    private float MovementSpeed = 3f;
    private IInputHandler InputHandler;
    private CharacterController _characterController;
    private HealthComponent _healthComponent;
    private Camera _camera;
    private Animator animator;
    private LineRenderer lineRenderer;
    private AudioConfig audioConfig;
    private IAudioService audioService;

    private float MoveThreshold = 0.001f;
    private IWeapon currentWeapon;
    public bool IsPaused { get; set; } = false;

    public Transform Transform => transform;

    public event Action OnDeath;

    [Inject]
    private void Construct(IInputHandler inputHandler, GameConfig gameConfig, HitscanWeapon weapon, AudioConfig audioConfig, IAudioService audioService)
    {
        InputHandler = inputHandler;
        MovementSpeed = gameConfig.PlayerMovementSpeed;
        this.currentWeapon = weapon;
        this.audioConfig = audioConfig;
        this.audioService = audioService;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _healthComponent = GetComponent<HealthComponent>();
        currentWeapon.SetOwner(weaponBarrel.transform);
        _healthComponent.OnDeath += OnPlayerDeath;
        animator = GetComponentInChildren<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnPlayerDeath()
    {
        OnDeath?.Invoke();
    }

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying || IsPaused)
        {
            return;
        }
        Vector2 moveAxis = InputHandler.MoveAxis();
        Vector3 motion = Vector3.zero;
        if (moveAxis.magnitude > MoveThreshold)
        {
            Vector3 moveVector = _camera.transform.TransformDirection(moveAxis);
            moveVector.y = 0;
            moveVector = moveVector.normalized;
            motion = MovementSpeed * Time.deltaTime * moveVector;
            _characterController.Move(motion);
        }
        animator.SetFloat("Speed", motion.magnitude);
        // Rotate character to aim direction
        Vector3 aimDirection = GetAimDirection();
        transform.forward = aimDirection;
        RenderAim(aimDirection);
        if (InputHandler.isFiring())
        {
            // attack using current weapon
            if (currentWeapon.Attack(aimDirection))
            {
                animator.SetTrigger("Shoot");
                audioService.PlayOneShot(audioConfig.GunShot, transform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || IsPaused)
        {
            return;
        }
        Vector3 aimDirection = GetAimDirection();
        Gizmos.DrawLine(transform.position, transform.position + aimDirection);
    }

    // Returns aim direction normalized
    private Vector3 GetAimDirection()
    {
        Vector3 start = transform.position;
        Vector2 look = InputHandler.LookAxis();
        Ray ray = _camera.ScreenPointToRay(look);
        Plane plane = new Plane(Vector3.up, start);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            return (point - start).normalized;
        }
        return Vector3.zero;
    }

    private void RenderAim(Vector3 aimDirection)
    {
        Vector3 raycastPoint = weaponBarrel.transform.position;
        Vector3 endPoint;
        lineRenderer.SetPosition(0, raycastPoint);
        float distance = 20f;
        if (Physics.Raycast(raycastPoint, aimDirection, out RaycastHit hit, distance))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = raycastPoint + new Vector3(aimDirection.x * distance, aimDirection.y, aimDirection.z * distance);
        }
        lineRenderer.SetPosition(1, endPoint);
    }

    public void TakeDamage(float damage)
    {
        _healthComponent.TakeDamage(damage);
    }

    public void OnFootstep()
    {
        audioService.PlayOneShot(audioConfig.PlayerFootstep, transform.position);
    }
}
