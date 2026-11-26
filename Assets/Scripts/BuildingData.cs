using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "City/Building")]
public class BuildingData : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    public int cost;
    public int capacity;
}
