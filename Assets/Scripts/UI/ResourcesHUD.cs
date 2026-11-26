using UnityEngine;
using UnityEngine.UIElements;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField] private PanelRenderer panelRenderer;
    [SerializeField] private CityResources resources;

    private Label moneyLabel;
    private Label populationLabel;

    private void OnEnable()
    {
        panelRenderer.RegisterUIReloadCallback(BindLabels);
        resources.OnChanged += Refresh;
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(BindLabels);
        resources.OnChanged -= Refresh;
    }

    private void BindLabels(PanelRenderer renderer, VisualElement root, int version)
    {
        moneyLabel = root.Q<Label>("money");
        populationLabel = root.Q<Label>("population");
        Refresh();
    }

    private void Refresh()
    {
        if (moneyLabel == null) return;

        moneyLabel.text = $"Argent : {resources.Money}";
        populationLabel.text = $"Nombre d'habitants : {resources.Population}/{resources.MaxPopulation}";
    }
}
