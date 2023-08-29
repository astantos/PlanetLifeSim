using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeColorGenerator
{
    protected Material mat;
    protected Texture2D texture;

    protected const int textureResolution = 50;

    public void UpdateGenerator(Material material)
    {
        mat = material;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void UpdateElevation(PlanetData planetData)
    {
        mat.SetVector("_elevationMinMax", new Vector4(planetData.MinAlt, planetData.MaxAlt));
    }

    public void UpdateColours(Gradient gradient)
    {
        Color[] colours = new Color[textureResolution];
        for (int index = 0; index < colours.Length; index++)
        {
            colours[index] = gradient.Evaluate(index / (textureResolution - 1f));
        }
        texture.SetPixels(colours);
        texture.Apply();
        mat.SetTexture("_planetTexture", texture);
    }
}
