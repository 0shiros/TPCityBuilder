using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private CityRessources ressources;
    [SerializeField] private ConstructionBar constructionBar;

    private BuildingData selected;
    private Vector2 mousePosition;

    private void OnEnable()
    {
        constructionBar.OnBuildingSelected += SetSelected;
        InputPlayer.OnMousePosition += SetMousePosition;
        InputPlayer.OnChoose += Place;
        InputPlayer.OnCancel += DestroyOrCancel;
    }

    private void OnDisable()
    {
        constructionBar.OnBuildingSelected -= SetSelected;
        InputPlayer.OnMousePosition -= SetMousePosition;
        InputPlayer.OnChoose -= Place;
        InputPlayer.OnCancel -= DestroyOrCancel;
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

        GameObject building = Instantiate(selected.prefab, cell.Center, Quaternion.identity);
        grid.Occupy(cell, building, selected);
    }

    private void DestroyOrCancel()
    {
        GridCell cell = grid.HoveredCell;

        if (cell != null && cell.IsOccupied && !constructionBar.IsPointerOverBar(mousePosition))
        {
            ressources.AddMoney(cell.BuildingData.cost / 2);
            Destroy(cell.Building);
            grid.Free(cell);
            return;
        }

        constructionBar.ClearSelection();
    }
}
