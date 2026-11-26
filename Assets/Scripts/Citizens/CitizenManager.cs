using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private CityRessources ressources;

    [SerializeField] private float minArrivalDelay = 15f;
    [SerializeField] private float maxArrivalDelay = 30f;

    [SerializeField] private int moneyPerCitizen = 100;
    [SerializeField] private float incomeInterval = 10f;

    private float nextIncomeTime;

    private void OnValidate()
    {
        minArrivalDelay = Mathf.Max(0f, minArrivalDelay);
        maxArrivalDelay = Mathf.Max(minArrivalDelay, maxArrivalDelay);
        incomeInterval = Mathf.Max(0.1f, incomeInterval);
    }

    private void Start()
    {
        nextIncomeTime = Time.time + incomeInterval;
    }

    private void Update()
    {
        int population = 0;

        foreach (GridCell cell in grid.Cells)
        {
            if (!cell.IsOccupied) continue;

            UpdateArrivals(cell);
            population += cell.Citizens;
        }

        ressources.SetPopulation(population);

        if (Time.time >= nextIncomeTime)
        {
            nextIncomeTime += incomeInterval;
            ressources.AddMoney(population * moneyPerCitizen);
        }
    }

    private void UpdateArrivals(GridCell cell)
    {
        if (cell.Citizens >= cell.BuildingData.GetCapacity(cell.Level))
        {
            cell.NextCitizenTime = 0f;
            return;
        }

        // 0 = pas encore programmé : maison neuve, citoyen tout juste arrivé, ou maison qui vient de gagner de la place.
        if (cell.NextCitizenTime == 0f)
        {
            cell.NextCitizenTime = Time.time + Random.Range(minArrivalDelay, maxArrivalDelay);
            return;
        }

        if (Time.time >= cell.NextCitizenTime)
        {
            cell.Citizens++;
            cell.NextCitizenTime = 0f;
        }
    }
}
