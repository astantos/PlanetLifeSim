using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    public int Resolution;

    protected Mesh mesh;
    protected Vector3 localUp;
    protected Vector3 axisA;
    protected Vector3 axisB;

    protected PlanetData planetData;
    protected AltitudeColorGenerator altitudeColorGenerator;

    public TerrainFace(Mesh mesh, int res, Vector3 up, PlanetData planetData, AltitudeColorGenerator altitudeColorGenerator)
    {
        this.mesh = mesh;
        this.planetData = planetData;
        this.altitudeColorGenerator = altitudeColorGenerator;

        Resolution = res;
        localUp = up;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh(float radius, Color baseColor, NoiseFilter[] filters, int seed)
    {
        Vector3[] vertices = new Vector3[Resolution * Resolution];
        int[] triangles = new int[(Resolution - 1) * (Resolution - 1) * 2 * 3];
        int triIndex = 0;
        Vector2[] uv = mesh.uv;

        int count = 0;
        for (int y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                Vector2 percent = new Vector2(x, y) / (Resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                float elevation = 0;
                for (int filter = 0; filter < filters.Length; filter++)
                {
                    int mask = filters[filter].FilterSettings.Mask;
                    float maskValue = 1;
                    if (mask != -1)
                    {
                        if (mask >= 0 && mask < filters.Length)
                        {
                            maskValue = filters[mask].Evaluate(pointOnUnitSphere, seed);
                        }
                        else
                        {
                            Debug.LogWarning("[ NOISE FILTER ] Warning: Mask Index must be a valid filter index");
                        }
                    }
                    elevation += filters[filter].Evaluate(pointOnUnitSphere, seed) * maskValue;
                }

                elevation = radius * (1 + elevation);
                planetData.RecordAltitudeExtremes(elevation);
                vertices[count] = pointOnUnitSphere * elevation;
                if (x < Resolution - 1 && y < Resolution - 1)
                {
                    triangles[triIndex] = count;
                    triangles[triIndex + 1] = count + Resolution + 1;
                    triangles[triIndex + 2] = count + Resolution;

                    triangles[triIndex + 3] = count;
                    triangles[triIndex + 4] = count + 1;
                    triangles[triIndex + 5] = count + Resolution + 1;

                    triIndex += 6;
                }

                count++;
            }
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.uv = uv;
    }

    public void UpdateUVs(Biome[] biomes, Planet.BiomeInfo biomeSettings)
    {
        Vector2[] uv = new Vector2[mesh.vertices.Length];

        for (int y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                int i = x + y * Resolution;
                Vector2 percent = new Vector2(x, y) / (Resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i] = new Vector2(altitudeColorGenerator.BiomePercentFromPoint(pointOnUnitSphere, biomes, biomeSettings), 0);
            }
        }

        mesh.uv = uv;
    }

}
