using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SliceTrigger : MonoBehaviour
{
    [SerializeField] private float minCuttingSpeedThreshold;
    private Transform parent;

    // Awake is called when the script instance is being loaded.
    protected void Awake()
    {
        parent = transform.parent;
        var bounds = GetComponent<BoxCollider>().bounds;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade") && GetVelocitySum(other.attachedRigidbody) > minCuttingSpeedThreshold)
        {
            var parent = transform.parent;
            GameObject firstHalf = new GameObject();
            firstHalf.name = "FirstHalf";
            GameObject secondHalf = new GameObject();
            secondHalf.name = "SecondHalf";
            firstHalf.transform.parent = secondHalf.transform.parent = parent.parent;
            Transform[] children = new Transform[parent.childCount];
            //Fill the array of children
            for (int i = 0; i < parent.childCount; i++)
            {
                children[i] = parent.GetChild(i);
            }

            //Get this trigger's sibling index
            int triggerIndex = transform.GetSiblingIndex();
            //Fill firstHalf's children
            for (int i = 0; i < triggerIndex; i++)
            {
                children[i].parent = firstHalf.transform;
            }

            //Fill secondHalf's children
            for (int i = triggerIndex + 1; i < children.Length; i++)
            {
                children[i].parent = secondHalf.transform;
            }

            firstHalf.AddComponent<Rigidbody>();
            secondHalf.AddComponent<Rigidbody>();
            Destroy(parent.gameObject);
        }
    }

    private float GetVelocitySum(Rigidbody bladeRigidbody)
    {
        Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();
        Vector3 bladeDirection = bladeRigidbody.transform.forward;
        float parentSpeedBladeDirection = Vector3.Project(parentRigidbody.velocity, -bladeDirection).magnitude;
        return bladeRigidbody.velocity.magnitude + parentSpeedBladeDirection;
    }
}