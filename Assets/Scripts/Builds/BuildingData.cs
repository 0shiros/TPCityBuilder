using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingUpgrade
{
    public int cost;
    public int capacity;
    [Tooltip("Facultatif : vide = on garde le modèle actuel.")]
    public Mesh mesh;
}

[CreateAssetMenu(fileName = "NewBuilding", menuName = "City/Building")]
public class BuildingData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public int cost;
    [Tooltip("0 = pas de capacité.")]
    public int capacity;

    [Tooltip("Niveaux 2 et suivants. Vide = le bâtiment n'évolue pas.")]
    public List<BuildingUpgrade> upgrades = new();
}
