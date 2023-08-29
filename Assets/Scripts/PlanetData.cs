using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetData
{
    public float MinAlt { get; protected set; }
    public float MaxAlt { get; protected set; }

    public PlanetData()
    {
        MinAlt = float.MaxValue;
        MaxAlt = float.MinValue;
    }

    public void RecordAltitudeExtremes(float value)
    {
        if (value > MaxAlt) MaxAlt = value;
        if (value < MinAlt) MinAlt = value;
    }
}
