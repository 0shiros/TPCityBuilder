using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionBar : MonoBehaviour
{
    private const string SelectedClass = "building-button--selected";

    [SerializeField] private UIDocument document;
    [SerializeField] private List<BuildingData> buildings = new();

    public event Action<BuildingData> OnBuildingSelected;

    private BuildingData selected;
    private Button selectedButton;

    private void OnEnable()
    {
        VisualElement container = document.rootVisualElement.Q<VisualElement>("buttons");
        container.Clear();

        foreach (BuildingData building in buildings)
        {
            Button button = new Button { text = building.displayName };
            button.AddToClassList("building-button");
            button.clicked += () => Select(building, button);
            container.Add(button);
        }
    }

    private void Select(BuildingData building, Button button)
    {
        selectedButton?.RemoveFromClassList(SelectedClass);

        // Recliquer sur le bâtiment déjà choisi le désélectionne.
        bool deselect = selected == building;
        selected = deselect ? null : building;
        selectedButton = deselect ? null : button;

        selectedButton?.AddToClassList(SelectedClass);
        OnBuildingSelected?.Invoke(selected);
    }
}
