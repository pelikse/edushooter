using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnDelay : MonoBehaviour
{
    public UnityEvent Methods;
    public float DefaultDelay = 2f;

    public void InvokeMethods()
    {
        StartCoroutine(DelayedInvocation());
    }

    private IEnumerator DelayedInvocation()
    {
        yield return new WaitForSeconds(DefaultDelay);

        Methods?.Invoke();
    }
}
