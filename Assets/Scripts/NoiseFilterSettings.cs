using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlanetGeneration/NoiseFilterSettings")]
public class NoiseFilterSettings : ScriptableObject
{
    [Range(1,8)]
    public int Layers;
    public float Strength;
    public float BaseRoughness;
    public float Roughness;
    public float Persistence;
    public float MinValue;
    public Vector3 Center;
}
