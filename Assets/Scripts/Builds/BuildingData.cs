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
    [Tooltip("Prefab avec un composant Building sur sa racine.")]
    public Building prefab;
    public int cost;
    [Tooltip("0 = pas de capacité.")]
    public int capacity;
    [Tooltip("Revenu de base par habitant, à chaque versement.")]
    public int incomePerCitizen;

    [Tooltip("Niveaux 2 et suivants. Vide = le bâtiment n'évolue pas.")]
    public List<BuildingUpgrade> upgrades = new();

    public int GetCapacity(int level)
    {
        return level <= 1 ? capacity : upgrades[level - 2].capacity;
    }
}
