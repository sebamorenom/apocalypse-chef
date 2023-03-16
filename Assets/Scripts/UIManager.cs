using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI fields")] [SerializeField] private TextMeshProUGUI scoreInUI;
    [SerializeField] private Animator[] starsAnimators;
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private Color preparationTimerColor;
    [SerializeField] private Color dayTimerColor;


    [SerializeField] private float[] scoreThresholds;

    [Header("Data inputs")] [SerializeField]
    private GameInfo gameInfo;

    [Header("Day parameters")] public float dayDuration;
    [SerializeField] public float preparationDuration;

    private int _numStarsActive = 0;
    private static readonly int Active = Animator.StringToHash("Active");

    private float _timeStartOfDay;
    private float _timeStartOfPreparation;

    private float _endDayTime;

    private bool _inPreparationTime;
    private bool _inGameTime;

    private float _currentTime;

    public bool dayNeedsToEnd;

    private Director _currentDir;

    private int _roundedTime, _minutes, _seconds;

    // Start is called before the first frame update
    void Start()
    {
        _currentDir = FindObjectOfType<Director>();
        scoreThresholds = gameInfo.difficultySettings.scoreThresholds;
        _inPreparationTime = true;
        _timeStartOfPreparation = Time.fixedTime;
        StartCoroutine(StartTimers());
    }

    // Update is called once per frame
    void Update()
    {
        scoreInUI.text = gameInfo.currentDayScore.ToString();
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

    public void ResetStars()
    {
        for (int i = 0; i < starsAnimators.Length; i++)
        {
            starsAnimators[i].SetBool(Active, false);
        }
    }

    public IEnumerator StartTimers()
    {
        timerUI.color = preparationTimerColor;
        _currentTime = _timeStartOfPreparation;
        while ((_currentTime < _timeStartOfPreparation + preparationDuration))
        {
            ToTimeFormat(_timeStartOfPreparation + preparationDuration - _currentTime);
            _currentTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        timerUI.color = dayTimerColor;
        _timeStartOfDay = _currentTime;
        while (_currentTime <= _timeStartOfDay + dayDuration)
        {
            ToTimeFormat(_timeStartOfPreparation + dayDuration - _currentTime);
            _currentTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        dayNeedsToEnd = true;
    }

    public void ToTimeFormat(float time)
    {
        _roundedTime = Mathf.RoundToInt(time);
        _seconds = _roundedTime % 60;
        _minutes = _roundedTime / 60;
        timerUI.text = $"{_minutes:00}:{_seconds:00}";
    }
}