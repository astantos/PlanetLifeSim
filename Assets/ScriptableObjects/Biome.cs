using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlanetGeneration/Biome")]
public class Biome : ScriptableObject
{
    [Range(0, 1)]
    public float StartHeight;

    [Space]
    public Gradient Gradient;
    public Color Tint;

    [Range(0,1)]
    public float TintPercent;
}
