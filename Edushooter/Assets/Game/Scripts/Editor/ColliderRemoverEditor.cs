using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColliderRemover))]
public class ColliderRemoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Reference to the target script
        ColliderRemover removeColliders = (ColliderRemover)target;

        // Add a button to the inspector
        if (GUILayout.Button("Remove All Colliders"))
        {
            removeColliders.StartRemoval();
        }
    }
}
