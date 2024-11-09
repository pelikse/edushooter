using System.Collections.Generic;

using UnityEngine;

using MoreMountains.Tools;


public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;

    private const string COMBINED_MESH_PATH = "Assets/CombinedMeshes/";


    [MMInspectorButton("DebugCombineMesh")]
    public bool MeshCombineBtn;
    public void DebugCombineMesh()
    {
        CombineMeshes();
    }

    private void CombineMeshes()
    {
        if (sourceMeshFilters == null || sourceMeshFilters.Count == 0)
        {
            Debug.LogError("Source Meshes are empty or not assigned!");
            return;
        }

        if (targetMeshFilter == null)
        {
            Debug.LogWarning("Target mesh is not set, getting mesh from parent...");
            targetMeshFilter = gameObject.GetComponent<MeshFilter>();

            if (targetMeshFilter == null)
            {
                Debug.LogError("No target mesh found!");
                return;
            }
        }

        // Combine meshes
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (var i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);

        // Center the combined mesh
        Vector3[] vertices = mesh.vertices;
        Vector3 center = mesh.bounds.center;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        targetMeshFilter.mesh = mesh;

        Debug.Log("Meshes combined and centered");
    }



}
