using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRemover : MonoBehaviour
{
    [SerializeField]
    private Transform ParentTransform;

    public void StartRemoval()
    {
        if (ParentTransform == null)
        {
            ParentTransform = gameObject.transform;
        }

        RemoveColliders(ParentTransform);
    }

    private void RemoveColliders(Transform parent)
    {
        Collider[] colliders = parent.GetComponents<Collider>();

        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        foreach (Transform children in parent)
        {
            RemoveColliders(children);
        }
    }
}
