using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseFilter
{
    public NoiseFilterSettings FilterSettings;

    public NoiseFilter(NoiseFilterSettings settings)
    {
        FilterSettings = settings;
    }

    public abstract float Evaluate(Vector3 point, int seed = 0);
}
