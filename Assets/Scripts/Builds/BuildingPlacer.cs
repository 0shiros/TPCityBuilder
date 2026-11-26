using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private CityResources resources;
    [SerializeField] private ConstructionBar constructionBar;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform buildingsParent;

    private BuildingData selected;
    private Vector2 mousePosition;

    private void OnEnable()
    {
        constructionBar.OnBuildingSelected += SetSelected;
        InputPlayer.OnMousePosition += SetMousePosition;
        InputPlayer.OnChoose += Place;
        InputPlayer.OnCancel += DestroyOrCancel;
        InputPlayer.OnUpgrade += UpgradeHovered;
    }

    private void OnDisable()
    {
        constructionBar.OnBuildingSelected -= SetSelected;
        InputPlayer.OnMousePosition -= SetMousePosition;
        InputPlayer.OnChoose -= Place;
        InputPlayer.OnCancel -= DestroyOrCancel;
        InputPlayer.OnUpgrade -= UpgradeHovered;
    }

    private void SetSelected(BuildingData building)
    {
        selected = building;
    }

    private void SetMousePosition(Vector2 position)
    {
        mousePosition = position;
    }

    private void Place()
    {
        GridCell cell = grid.HoveredCell;
        if (selected == null || constructionBar.IsPointerOverBar(mousePosition) || !grid.IsFree(cell)) return;

        if (!resources.TrySpend(selected.cost))
        {
            Debug.Log($"Pas assez d'argent pour {selected.displayName} ({selected.cost} $).");
            return;
        }

        Building building = Instantiate(selected.prefab, cell.Center, FacingCamera(), buildingsParent);
        building.Init(selected);
        grid.Occupy(cell, building);
    }

    private void UpgradeHovered()
    {
        GridCell cell = grid.HoveredCell;
        
        if (cell != null && cell.IsOccupied)
        {
            Upgrade(cell);
        }
    }

    private void Upgrade(GridCell cell)
    {
        Building building = cell.Building;

        if (building.Level - 1 >= building.Data.upgrades.Count)
        {
            return;
        }

        BuildingUpgrade upgrade = building.Data.upgrades[building.Level - 1];

        if (!resources.TrySpend(upgrade.cost))
        {
            return;
        }

        building.Level++;
        building.TotalSpent += upgrade.cost;

        if (upgrade.mesh == null || building.MeshFilter == null) return;

        building.MeshFilter.mesh = upgrade.mesh;
    }

    private Quaternion FacingCamera()
    {
        float yawToCamera = cameraTransform.eulerAngles.y + 180f;
        return Quaternion.Euler(0f, Mathf.Round(yawToCamera / 90f) * 90f, 0f);
    }

    private void DestroyOrCancel()
    {
        if (selected != null)
        {
            constructionBar.ClearSelection();
            return;
        }

        GridCell cell = grid.HoveredCell;

        if (cell == null || !cell.IsOccupied || constructionBar.IsPointerOverBar(mousePosition)) return;
        
        resources.AddMoney(cell.Building.TotalSpent / 2);
        Destroy(cell.Building.gameObject);
        grid.Free(cell);
    }
}
