using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    [SerializeField] public string difficultyName;

    [Header("Score fields")] [SerializeField]
    public float[] scoreThresholds = new float[5];

    [SerializeField] public float scoreThresholdScalingModifier;

    [Header("AI Director fields")] [SerializeField]
    public float startingPoints;

    [SerializeField] public float pointScalingModifier;

    private void OnValidate()
    {
        if (scoreThresholds.Length != 5)
        {
            Debug.LogError("There needs to be 5 score thresholds, one for each of the stars");
        }
    }
}