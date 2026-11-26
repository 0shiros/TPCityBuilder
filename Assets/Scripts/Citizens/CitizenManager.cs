using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private CityResources resources;

    [SerializeField] private float ArrivalDelay = 15f;

    [SerializeField] private float incomeInterval = 10f;

    private float nextIncomeTime;

    private void Start()
    {
        nextIncomeTime = Time.time + incomeInterval;
    }

    private void Update()
    {
        int population = 0;
        int maxPopulation = 0;

        foreach (GridCell cell in grid.Cells)
        {
            Building building = cell.Building;
            if (building == null) continue;

            maxPopulation += building.GetCapacity();
            if (building.Habitation == null) continue;

            UpdateArrivals(building);
            population += building.Habitation.Citizens;
        }

        resources.SetPopulation(population, maxPopulation);

        if (Time.time >= nextIncomeTime)
        {
            nextIncomeTime += incomeInterval;
            resources.AddMoney(CollectIncome());
        }
    }

    private int CollectIncome()
    {
        GridCell[,] cells = grid.Cells;

        foreach (GridCell cell in cells)
        {
            Habitation habitation = GetHabitation(cell);
            if (habitation != null)
            {
                habitation.IncomeBonus = 0;
            }
        }

        foreach (GridCell source in cells)
        {
            if (source.Building == null || source.Building.Enviro == null) continue;

            BuildingEnviro enviro = source.Building.Enviro;
            for (int dx = -enviro.Radius; dx <= enviro.Radius; dx++)
            {
                for (int dz = -enviro.Radius; dz <= enviro.Radius; dz++)
                {
                    GridCell target = grid.GetCell(source.Coordinates + new Vector2Int(dx, dz));
                    Habitation habitation = GetHabitation(target);
                    if (habitation != null)
                    {
                        habitation.IncomeBonus += enviro.IncomeBonusPerCitizen;
                    }
                }
            }
        }

        int income = 0;
        foreach (GridCell cell in cells)
        {
            Habitation habitation = GetHabitation(cell);
            if (habitation == null) continue;

            income += habitation.Citizens * (cell.Building.Data.incomePerCitizen + habitation.IncomeBonus);
        }
        return income;
    }

    private static Habitation GetHabitation(GridCell cell)
    {
        return cell != null && cell.Building != null ? cell.Building.Habitation : null;
    }

    private void UpdateArrivals(Building building)
    {
        Habitation habitation = building.Habitation;

        if (habitation.Citizens >= building.GetCapacity())
        {
            habitation.NextCitizenTime = 0f;
            return;
        }

        // 0 = pas encore programmé : maison neuve, citoyen tout juste arrivé, ou maison qui vient de gagner de la place.
        if (habitation.NextCitizenTime == 0f)
        {
            habitation.NextCitizenTime = Time.time + ArrivalDelay;
            return;
        }

        if (Time.time >= habitation.NextCitizenTime)
        {
            habitation.Citizens++;
            habitation.NextCitizenTime = 0f;
        }
    }
}
