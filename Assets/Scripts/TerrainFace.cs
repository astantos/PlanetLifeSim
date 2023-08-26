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

    public TerrainFace(Mesh mesh, int res, Vector3 up)
    {
        this.mesh = mesh;
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
                    elevation += filters[filter].Evaluate(pointOnUnitSphere, seed);
                }
                vertices[count] = pointOnUnitSphere * radius * (1 + elevation);
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
    }
}
