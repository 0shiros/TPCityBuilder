using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 50f;
    private Vector3 MovementDirection;

    [SerializeField] private float zoomStep = 5f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 150f;

    [SerializeField] private float limit = 250f;
    private Vector3 offset;

    public bool canMove = true;
    public bool canZoom = true;

    private void Awake()
    {
        Vector3 lookPoint = transform.position + transform.forward * (transform.position.y / -transform.forward.y);
        offset = transform.position - lookPoint;
    }

    private void OnEnable()
    {
        InputPlayer.OnMove += GetMovementDirection;
        InputPlayer.OnZoom += Zoom;
    }

    private void OnDisable()
    {
        InputPlayer.OnMove -= GetMovementDirection;
        InputPlayer.OnZoom -= Zoom;
    }

    private void Update()
    {
        if(canMove)
        {
            Movement();
        }
    }

    private void GetMovementDirection(Vector3 direction)
    {
        MovementDirection = new Vector3(direction.x, 0, direction.y);
    }

    private void Movement()
    {
        Vector3 direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * MovementDirection;
        Vector3 lookPoint = transform.position - offset + direction * (Time.deltaTime * speed);
        lookPoint.x = Mathf.Clamp(lookPoint.x, -limit, limit);
        lookPoint.z = Mathf.Clamp(lookPoint.z, -limit, limit);
        transform.position = lookPoint + offset;
    }

    private void Zoom(float direction)
    {
        if (!canZoom || direction == 0) return;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Mathf.Sign(direction) * zoomStep, minZoom, maxZoom);
    }
}
