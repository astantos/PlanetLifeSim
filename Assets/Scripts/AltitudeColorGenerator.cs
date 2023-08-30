using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeColorGenerator
{
    protected Material planetMaterial;
    protected Texture2D texture;

    protected const int textureResolution = 50;

    protected SimpleNoiseFilter biomeNoiseFilter;

    public AltitudeColorGenerator(Material material, Planet.BiomeInfo biomeSettings)
    {
        planetMaterial = material;
        biomeNoiseFilter = new SimpleNoiseFilter(biomeSettings.BiomeNoiseSettings);
    }

    public void UpdateColorGenerator(Biome[] biomes)
    {
        if (texture == null || texture.height != biomes.Length)
        {
            texture = new Texture2D(textureResolution, biomes.Length);
            Debug.Log($"[ ALTITUDE COLOR GENERATOR ] Texture Height: {biomes.Length}");
        }
    }

    public void UpdateElevation(PlanetData planetData)
    {
        planetMaterial.SetVector("_elevationMinMax", new Vector4(planetData.MinAlt, planetData.MaxAlt));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere, Biome[] biomes, Planet.BiomeInfo biomeSettings)
    {
        if (biomes.Length == 1) return 1;

        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - biomeSettings.Offset) * biomeSettings.Strength; 
        float biomeIndex = 0;
        float blendRange = biomeSettings.BlendAmount / 2f + 0.001f;
        for(int index = 0; index < biomes.Length; index++)
        {
            float distance = heightPercent - biomes[index].StartHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= 1 - weight;
            biomeIndex += index * weight;
        }

        return biomeIndex / (biomes.Length - 1);
    }

    public void UpdateColours(Gradient gradient, Biome[] biomes)
    {
        Color[] colors = new Color[texture.width * texture.height];
        Debug.Log($"[ ALTITUDE COLOR GENERATOR ] Update Colors - Texture Height: {biomes.Length}");

        int colorIndex = 0;
        for (int b = 0; b < biomes.Length; b++)
        {
            for (int index = 0; index < textureResolution; index++)
            {
                Biome biome = biomes[b];
                Color gradientColor = biome.Gradient.Evaluate((index / (textureResolution - 1f)));
                Color tintColor = biome.Tint;
                colors[colorIndex] = gradientColor * (1 - biome.TintPercent) + tintColor * biome.TintPercent;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        planetMaterial.SetTexture("_planetTexture", texture);
    }
}
