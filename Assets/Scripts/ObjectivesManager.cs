using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] public List<Transform> objectivesTransform;
    [SerializeField] public List<Health> objectivesHealth;

    public void RemoveFromArrays(Transform objTransform, Health objHealth)
    {
        objectivesTransform.Remove(objTransform);
        objectivesHealth.Remove(objHealth);
    }
}