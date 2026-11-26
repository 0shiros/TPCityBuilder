using UnityEngine;

// Données d'une maison posée. Pas d'Update : c'est CitizenManager qui les lit et les modifie.
public class Habitation : MonoBehaviour
{
    [SerializeField] private int citizens;
    [SerializeField] private float nextCitizenTime;
    [SerializeField] private int incomeBonus;

    public int Citizens
    {
        get => citizens;
        set => citizens = value;
    }

    public float NextCitizenTime
    {
        get => nextCitizenTime;
        set => nextCitizenTime = value;
    }

    public int IncomeBonus
    {
        get => incomeBonus;
        set => incomeBonus = value;
    }
}
