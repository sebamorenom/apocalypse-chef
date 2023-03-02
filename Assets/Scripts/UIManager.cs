using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI fields")] [SerializeField] private TextMeshProUGUI scoreInUI;
    [SerializeField] private Animator[] starsAnimators;

    [SerializeField] private float[] scoreThresholds;

    [Header("Data inputs")] [SerializeField]
    private GameInfo gameInfo;

    // Start is called before the first frame update
    void Start()
    {
        scoreThresholds = gameInfo.GetScoreThresholds();
    }

    // Update is called once per frame
    void Update()
    {
    }
}