using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int Resolution;

    protected GameObject[] faces;

    protected MeshRenderer[] meshRenderers;
    protected MeshFilter[] meshFilters;

    protected TerrainFace[] terrainFaces;
    
    #region Initialization
    public void Initialize()
    {
        InitializeArrays();

        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int index = 0; index < meshFilters.Length; index++)
        {
            GameObject mesh = faces[index];
            meshRenderers[index].sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilters[index].sharedMesh = new Mesh();
            terrainFaces[index] = new TerrainFace(meshFilters[index].sharedMesh, Resolution, directions[index]);
        }
    }

    protected void InitializeArrays()
    {
        int count = 6;
        if (faces == null || faces.Length < count) faces = new GameObject[count];
        if (meshRenderers == null || meshRenderers.Length < count) meshRenderers = new MeshRenderer[count];
        if (meshFilters == null || meshFilters.Length < count) meshFilters = new MeshFilter[count];
        if (terrainFaces == null || terrainFaces.Length < count) terrainFaces = new TerrainFace[count];

        for (int index = 0; index < count; index++)
        {
            if (faces[index] == null)
            {
                GameObject newMesh = new GameObject("mesh");
                faces[index] = newMesh;
                newMesh.transform.parent = transform;
                meshRenderers[index] = faces[index].AddComponent<MeshRenderer>();
                meshFilters[index] = faces[index].AddComponent<MeshFilter>();
            }
        }
    }

    protected void GenerateMesh()
    {
        for (int index = 0; index < terrainFaces.Length; index++)
        {
            terrainFaces[index].ConstructMesh();
        }
    }
    #endregion

    #region Unity Functions
    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }
    #endregion
}
