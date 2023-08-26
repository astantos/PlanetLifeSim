using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseFilter
{
    public bool Enabled;
    public NoiseFilterSettings FilterSettings;

    public float Evaluate(Vector3 point, int seed = 0)
    {

        if (Enabled == false) return 0; 

        if (FilterSettings == null)
        {
            Debug.LogWarning("[ NOISE FILTER ] Warning: Filter Settings is null!");
            return 0;
        }

        Noise noise = new Noise(seed);

        float noiseValue = 0;
        float frequency = FilterSettings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < FilterSettings.Layers; i++)
        {
            float v = noise.Evaluate(point * frequency + FilterSettings.Center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= FilterSettings.Roughness;
            amplitude *= FilterSettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - FilterSettings.MinValue);

        return noiseValue * FilterSettings.Strength;
    }
}
