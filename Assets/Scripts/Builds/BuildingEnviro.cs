using UnityEngine;

public class BuildingEnviro : MonoBehaviour
{
    [Tooltip("Ajouté au revenu par habitant des maisons dans le rayon.")]
    [SerializeField] private int incomeBonusPerCitizen = 25;
    [Tooltip("En cases, diagonales comprises.")]
    [SerializeField] private int radius = 1;

    public int IncomeBonusPerCitizen => incomeBonusPerCitizen;
    public int Radius => radius;
}
