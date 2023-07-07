using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseFilter
{
    [Range(1,8)]
    public int Layers;
    public float Strength;
    public float BaseRoughness;
    public float Roughness;
    public float Persistence;
    public Vector3 Center;

    public float Evaluate(Vector3 point)
    {
        Noise noise = new Noise();

        float noiseValue = 0;
        float frequency = BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < Layers; i++)
        {
            float v = noise.Evaluate(point * frequency + Center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= Roughness;
            amplitude *= Persistence;
        }

        return noiseValue * Strength;
    }
}
