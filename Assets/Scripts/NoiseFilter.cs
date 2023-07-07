using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseFilter
{
    public float Strength = 1;
    public float Roughness = 1;
    public Vector3 Center;

    public float Evaluate(Vector3 point)
    {
        Noise noise = new Noise();
        float noiseValue = (noise.Evaluate(point * Roughness + Center) + 1) * 0.5f;
        return noiseValue * Strength;
    }
}
