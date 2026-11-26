using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject objectToPlace;
    private GameObject ghostObject;
    [SerializeField] private int gridSize = 1;
    private HashSet<Vector3> occupiedPositions = new();
    
    private void Start()
    {
        CreateGhostObject();
    }
    
    private void Update()
    {
        UpdateGhostObjectPosition();
    }

    void CreateGhostObject()
    {
        if (ghostObject == null)
        {
            ghostObject = Instantiate(objectToPlace);
            ghostObject.GetComponent<Collider>().enabled = false; 
            
            Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 0.5f; 
                material.color = color;
                
                material.SetFloat("_Mode", 2);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
        }
    }

    void UpdateGhostObjectPosition()
    {
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(raycast, out RaycastHit hitInfo))
        {
            Vector3 point = hitInfo.point;
            Vector3 snappedPosition = new Vector3(
                Mathf.Round(point.x / gridSize) * gridSize,
                Mathf.Round(point.y / gridSize) * gridSize,
                Mathf.Round(point.z / gridSize) * gridSize
            );
            
            ghostObject.transform.position = snappedPosition;

            SetGhostObjectColor(occupiedPositions.Contains(snappedPosition) ? Color.red : Color.green);
        }
    }

    private void SetGhostObjectColor(Color color)
    {
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.color = color;
        }
    }
    
    void PlaceObject()
    {
        Vector3 placementPosition = ghostObject.transform.position;
        
        if (!occupiedPositions.Contains(placementPosition))
        {
            Instantiate(objectToPlace, placementPosition, Quaternion.identity);
            occupiedPositions.Add(placementPosition);
        }
    }
}
