using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionBar : MonoBehaviour
{
    private const string SelectedClass = "building-button--selected";

    [SerializeField] private PanelRenderer panelRenderer;
    [SerializeField] private List<BuildingData> buildings = new();

    public event Action<BuildingData> OnBuildingSelected;

    private VisualElement bar;
    private BuildingData selected;
    private Button selectedButton;

    private void OnEnable()
    {
        panelRenderer.RegisterUIReloadCallback(BuildButtons);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(BuildButtons);
    }

    public void ClearSelection()
    {
        if (selected != null)
        {
            Select(selected, selectedButton);
        }
    }

    public bool IsPointerOverBar(Vector2 screenPosition)
    {
        if (bar?.panel == null) return false;

        // Écran : origine en bas à gauche. Panel : origine en haut à gauche.
        Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(bar.panel, new Vector2(screenPosition.x, Screen.height - screenPosition.y));
        VisualElement picked = bar.panel.Pick(panelPosition);
        return picked != null && (picked == bar || bar.Contains(picked));
    }

    private void BuildButtons(PanelRenderer renderer, VisualElement root, int version)
    {
        selected = null;
        selectedButton = null;

        bar = root.Q<VisualElement>("buttons");
        bar.Clear();

        foreach (BuildingData building in buildings)
        {
            Button button = new Button();
            button.AddToClassList("building-button");

            if (building.icon != null)
            {
                VisualElement icon = new VisualElement();
                icon.AddToClassList("building-icon");
                icon.style.backgroundImage = new StyleBackground(building.icon);
                button.Add(icon);
            }

            button.Add(new Label($"{building.displayName}\n{building.cost} $"));
            button.clicked += () => Select(building, button);
            bar.Add(button);
        }
    }

    private void Select(BuildingData building, Button button)
    {
        selectedButton?.RemoveFromClassList(SelectedClass);

        bool deselect = selected == building;
        selected = deselect ? null : building;
        selectedButton = deselect ? null : button;

        selectedButton?.AddToClassList(SelectedClass);
        OnBuildingSelected?.Invoke(selected);
    }
}
