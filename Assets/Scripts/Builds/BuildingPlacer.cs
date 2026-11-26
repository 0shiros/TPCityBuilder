using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private CityRessources ressources;
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

        if (!ressources.TrySpend(selected.cost))
        {
            Debug.Log($"Pas assez d'argent pour {selected.displayName} ({selected.cost} $).");
            return;
        }

        GameObject building = Instantiate(selected.prefab, cell.Center, FacingCamera(), buildingsParent);
        grid.Occupy(cell, building, selected);
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
        BuildingData data = cell.BuildingData;

        if (cell.Level - 1 >= data.upgrades.Count)
        {
            return;
        }

        BuildingUpgrade upgrade = data.upgrades[cell.Level - 1];
        
        if (!ressources.TrySpend(upgrade.cost))
        {
            return;
        }

        cell.Level++;
        cell.TotalSpent += upgrade.cost;

        if (upgrade.prefab == null) return;
        
        Destroy(cell.Building);
        cell.Building = Instantiate(upgrade.prefab, cell.Center, FacingCamera(), buildingsParent);
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
        
        ressources.AddMoney(cell.TotalSpent / 2);
        Destroy(cell.Building);
        grid.Free(cell);
    }
}
