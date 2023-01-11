using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceTrigger : MonoBehaviour
{
    [SerializeField] private Transform piece;
    private Transform parent;

    // Awake is called when the script instance is being loaded.
    protected void Awake()
    {
        parent = transform.parent;
        var bounds = GetComponent<BoxCollider>().bounds;
    }

    // OnTriggerEnter is called when the Collider other enters the trigger.
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            var oldParent = piece.parent;
            GameObject newSlice = new GameObject();
            Transform[] children = parent.GetComponentsInChildren<Transform>();
            
            piece.parent = transform.parent.parent;
            var coll = piece.GetComponent<BoxCollider>();
            coll.enabled = true;
            Physics.IgnoreCollision(coll, oldParent.GetComponent<Collider>());

            piece.gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject);
        }
    }
}