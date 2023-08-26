using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Seed")]
    public int Seed;
    
    [Header("Planet Settings")]
    [Range(2, 256)]
    public int Resolution;
    public float Radius;
    public Color BaseColor;

    [Space]
    public NoiseFilterSettings[] NoiseFilterSettings;
    protected NoiseFilter[] NoiseFilters;
    
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
    
    protected void InitializeNoiseFilters()
    {
        NoiseFilter[] filters = new NoiseFilter[NoiseFilterSettings.Length];
        if (NoiseFilters == null)
        {
            NoiseFilters = new NoiseFilter[filters.Length];
        }

        for (int index = 0; index < NoiseFilterSettings.Length; index++)
        {
            if (index < NoiseFilters.Length && NoiseFilters[index] != null)
            {
                filters[index] = NoiseFilterFactory.CreateNoiseFilter(NoiseFilters[index], NoiseFilterSettings[index]);
            }
            else
            {
                filters[index] = NoiseFilterFactory.CreateNoiseFilter(NoiseFilterSettings[index]);
            }
        }
        NoiseFilters = filters;
    }
    #endregion

    #region Generation
    public void RegenerateAll()
    {
        Initialize();
        InitializeNoiseFilters();
        GenerateMesh();
        GenerateColors();
    }

    protected void GenerateMesh()
    {
        for (int index = 0; index < terrainFaces.Length; index++)
        {
            terrainFaces[index].ConstructMesh(Radius, BaseColor, NoiseFilters, Seed);
        }
    }

    protected void GenerateColors()
    {
        for (int index = 0; index < meshRenderers.Length; index++)
        {
            meshRenderers[index].sharedMaterial.color = BaseColor;
        }
    }
    #endregion

    #region Unity Functions
    private void OnValidate()
    {
        Debug.Log("[ >>>>> ] On Validate!");
        RegenerateAll();
    }
    #endregion
}
