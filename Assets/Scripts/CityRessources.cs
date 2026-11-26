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
    [SerializeField] private int maxPopulation;
    
    [SerializeField] private int money;
    [SerializeField] private int maxMoney;
    
    [SerializeField] private CurrentSpeed currentSpeed;
    
    private void UpdateSpeedGame()
    {
        Time.timeScale = (int)currentSpeed;
    }
    
}
