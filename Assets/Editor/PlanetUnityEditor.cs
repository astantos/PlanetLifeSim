using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetUnityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Planet planetScript = (Planet)target;

        if (GUILayout.Button("RegeneratePlanet"))
        {
            planetScript.RegenerateAll();
        }
    }
}
