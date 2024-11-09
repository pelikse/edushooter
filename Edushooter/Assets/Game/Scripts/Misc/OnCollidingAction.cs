using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollidingAction : MonoBehaviour
{
    [SerializeField] private Collider colliderHitbox;

    public UnityEvent OnCollision;
    public string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        if (colliderHitbox == null)
        {
            colliderHitbox = GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) || targetTag == "")
        {
            OnCollision?.Invoke();
        }
    }
}
