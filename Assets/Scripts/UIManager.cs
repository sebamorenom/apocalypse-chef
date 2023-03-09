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

    [Header("Day parameters")] public float dayDuration;

    private int _numStarsActive = 0;
    private static readonly int Active = Animator.StringToHash("Active");

    private float timeStartOfDay;

    public bool dayNeedsToEnd;

    private Director _currentDir;

    // Start is called before the first frame update
    void Start()
    {
        _currentDir = FindObjectOfType<Director>();
        scoreThresholds = gameInfo.difficultySettings.scoreThresholds;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInfo.currentDayScore >= scoreThresholds[_numStarsActive])
        {
            _numStarsActive++;
        }

        if (Time.fixedTime - timeStartOfDay > dayDuration)
        {
            dayNeedsToEnd = true;
        }
    }


    public void UpdateStarsState()
    {
        for (int i = 0; i < _numStarsActive; i++)
        {
            starsAnimators[i].SetBool(Active, true);
        }
    }

    public void ResetStars()
    {
        for (int i = 0; i < starsAnimators.Length; i++)
        {
            starsAnimators[i].SetBool(Active, false);
        }
    }

    public void StartDay()
    {
        timeStartOfDay = Time.fixedTime;
    }
    
}