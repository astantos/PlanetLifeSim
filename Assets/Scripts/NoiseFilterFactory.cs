using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static NoiseFilter CreateNoiseFilter(NoiseFilterSettings settings)
    {
        switch(settings.FilterType)
        {
            case NoiseFilterSettings.NoiseFilterType.Ridged:
                return new RidgedNoiseFilter(settings);
            default:
                return new SimpleNoiseFilter(settings);
        }
    }

    public static NoiseFilter CreateNoiseFilter(NoiseFilter filter, NoiseFilterSettings settings)
    {
        switch (settings.FilterType)
        {
            case NoiseFilterSettings.NoiseFilterType.Ridged:
                if (filter is RidgedNoiseFilter)
                {
                    filter.FilterSettings = settings;
                    return filter;
                }
                else
                {
                    return CreateNoiseFilter(settings);
                }
            default:
                if (filter is SimpleNoiseFilter)
                {
                    filter.FilterSettings = settings;
                    return filter;
                }
                else
                {
                    return CreateNoiseFilter(settings);
                }
        }
    }

}
