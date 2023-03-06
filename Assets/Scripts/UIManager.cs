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

    private int _numStarsActive = 0;
    private static readonly int Active = Animator.StringToHash("Active");

    // Start is called before the first frame update
    void Start()
    {
        scoreThresholds = gameInfo.difficultySettings.scoreThresholds;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInfo.currentDayScore >= scoreThresholds[_numStarsActive])
        {
            _numStarsActive++;
        }
    }


    public void UpdateStarsState()
    {
        for (int i = 0; i < _numStarsActive; i++)
        {
            starsAnimators[i].SetBool(Active, true);
        }
    }

    public void EndDay()
    {
        for (int i = 0; i < starsAnimators.Length; i++)
        {
            starsAnimators[i].SetBool(Active, false);
        }
    }
}