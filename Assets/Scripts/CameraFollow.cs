using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Vector3 offset = new Vector3(-7, 10, -7);
    [SerializeField]
    [Range(0f, 0.5f)]
    private float deadZoneRadius = 0.01f; 
    [SerializeField]
    private float followSpeed = 1f;

    private Camera _camera;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 playerScreenPos = _camera.WorldToViewportPoint(player.transform.position);
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Vector2 playerViewportPos = new Vector2(playerScreenPos.x, playerScreenPos.y);

        float distanceFromCenter = Vector2.Distance(playerViewportPos, screenCenter);

        if (distanceFromCenter > deadZoneRadius)
        {
            Vector3 position = player.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, position, followSpeed * Time.deltaTime);
        }
    }
}
