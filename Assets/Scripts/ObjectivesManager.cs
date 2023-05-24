using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Objective
{
    public Transform objectiveTransform;
    public Health objectiveHealth;
}

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] public List<Objective> priorityObjectives;
    [SerializeField] public List<Objective> objectives;

    private int _randomInt;

    public void RemoveFromArrays(Objective objectiveToRemove)
    {
        objectives.Remove(objectiveToRemove);
    }

    public Objective? GetRandomObjective()
    {
        _randomInt = Random.Range(0, objectives.Count);
        if (priorityObjectives.Count > 0)
        {
            return priorityObjectives[_randomInt % priorityObjectives.Count];
        }
        else if (objectives.Count > 0)
        {
            return objectives[_randomInt];
        }

        return null;
    }

    public bool Contains(Objective objectiveToCheck)
    {
        return (priorityObjectives.Contains(objectiveToCheck) || objectives.Contains(objectiveToCheck));
    }
}