using UnityEngine;

// Données d'un bâtiment posé, à mettre sur la racine du prefab. Pas d'Update.
public class Building : MonoBehaviour
{
    [Header("Composants du prefab (facultatifs)")]
    [SerializeField] private Habitation habitation;
    [SerializeField] private BuildingEnviro enviro;
    [SerializeField] private MeshFilter meshFilter;

    [Header("État en jeu")]
    [SerializeField] private BuildingData data;
    [SerializeField] private int level;
    [SerializeField] private int totalSpent;

    public BuildingData Data => data;
    public Habitation Habitation => habitation;
    public BuildingEnviro Enviro => enviro;
    public MeshFilter MeshFilter => meshFilter;

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int TotalSpent
    {
        get => totalSpent;
        set => totalSpent = value;
    }

    public void Init(BuildingData buildingData)
    {
        data = buildingData;
        level = 1;
        totalSpent = buildingData.cost;
    }

    public int GetCapacity()
    {
        return data.GetCapacity(level);
    }
}
