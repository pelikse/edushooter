using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectOnInvocation : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;

    public bool ActiveOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.SetActive(ActiveOnStart);
    }

    public void ActivateObject()
    {
        GameObject.SetActive(true);
    }

    public void DeactivateObject()
    {
        GameObject.SetActive(false);
    }
}
