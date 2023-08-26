using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RidgedNoiseFilter : NoiseFilter
{
    #region Astract Class Parameters
    // public bool Enabled;
    // public NoiseFilterSettings FilterSettings;
    // public int Mask = -1;
    #endregion

    public RidgedNoiseFilter(NoiseFilterSettings settings) : base(settings) { }

    public override float Evaluate(Vector3 point, int seed = 0)
    {
        if (FilterSettings.Enabled == false) return 0; 
        if (FilterSettings == null)
        {
            Debug.LogWarning("[ NOISE FILTER ] Warning: Filter Settings is null!");
            return 0;
        }

        Noise noise = new Noise(seed);

        float noiseValue = 0;
        float frequency = FilterSettings.BaseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < FilterSettings.Layers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + FilterSettings.Center));
            v *= v;
            v *= weight;
            weight = v;
            noiseValue += v * amplitude;
            frequency *= FilterSettings.Roughness;
            amplitude *= FilterSettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - FilterSettings.MinValue);

        return noiseValue * FilterSettings.Strength;
    }
}
