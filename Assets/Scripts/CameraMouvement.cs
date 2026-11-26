using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 MovementDirection;
    
    private float zoomSpeed = 10f;
    private float ZoomDirection;
    
    public bool canMove = true;
    public bool canZoom = true;
    
    private void OnEnable()
    {
        InputPlayer.OnMove += GetMovementDirection;
        InputPlayer.OnZoom += GetZoomDirection;
    }

    private void OnDisable()
    {
        InputPlayer.OnMove -= GetMovementDirection;
        InputPlayer.OnZoom -= GetZoomDirection;
    }

    private void Update()
    {
        if(canMove)
        {
            Movement();
        }
        
        if(canZoom)
        {
            Zoom();
        }
    }

    private void GetMovementDirection(Vector3 direction)
    {
        MovementDirection = new Vector3(direction.x, 0, direction.y);
    }
    
    private void Movement()
    {
        transform.position += MovementDirection * (Time.deltaTime * speed);
    }
        
    private void GetZoomDirection(float direction)
    {
       ZoomDirection = direction * (Time.deltaTime * zoomSpeed);
    }
    
    private void Zoom()
    {
        transform.position += transform.forward * ZoomDirection;
    }
}
