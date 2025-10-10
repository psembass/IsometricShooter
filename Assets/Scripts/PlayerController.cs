using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 3f;
    [SerializeField]
    private InputHandler InputHandler;
    private CharacterController _characterController;
    private Camera _camera;

    private float MoveThreshold = 0.001f;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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
    }
}
