using System;
using UnityEngine;

public enum CurrentSpeed
{
    Normal = 1,
    Fast = 2,
    VeryFast = 4
}


public class CityRessources : MonoBehaviour
{
    [SerializeField] private int population;
    [Tooltip("Calculé : somme des capacités des maisons.")]
    [SerializeField] private int maxPopulation;

    [SerializeField] private int money;

    [SerializeField] private CurrentSpeed currentSpeed;

    public event Action OnChanged;

    public int Money => money;
    public int Population => population;
    public int MaxPopulation => maxPopulation;

    public bool TrySpend(int amount)
    {
        if (money < amount) return false;

        money -= amount;
        OnChanged?.Invoke();
        return true;
    }

    public void SetPopulation(int value, int max)
    {
        if (value == population && max == maxPopulation) return;

        population = value;
        maxPopulation = max;
        OnChanged?.Invoke();
    }

    public void AddMoney(int amount)
    {
        if (amount == 0) return;

        money += amount;
        OnChanged?.Invoke();
    }

    private void UpdateSpeedGame()
    {
        Time.timeScale = (int)currentSpeed;
    }
    
}
