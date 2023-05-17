using System.Collections;
using System.Collections.Generic;
using ModularMotion;
using TMPro;
using UnityEngine;

public struct ScreenAnimations
{
    public static int ToMainSlide = Animator.StringToHash("ToMainSlide");
    public static int ToSecondarySlide = Animator.StringToHash("ToSecondarySlide");
}

public class UIManager : MonoBehaviour
{
    [Header("UI fields")] [SerializeField] private TextMeshProUGUI scoreInUI;
    [SerializeField] private Animator screenAnimator;
    [SerializeField] private UIMotion[] starsAnimators;
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private Color preparationTimerColor;
    [SerializeField] private Color dayTimerColor;


    [SerializeField] public int[] scoreThresholds;

    [Header("Data Inputs")] [SerializeField]
    public GameInfo gameInfo;


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

    private int _roundedTime, _minutes, _seconds;

    private string _minutesString, _secondsString;
    [HideInInspector] public bool canSpawnZombies;

    // Start is called before the first frame update
    void Start()
    {
        ResetStars();
        Director.Instance.uiManager = this;
        gameInfo = Director.Instance.currentGameInfo;
        scoreThresholds = gameInfo.scoreThresholds;
        //scoreThresholds = gameInfo.difficultySettings.scoreThresholds;
        _inPreparationTime = true;
        _timeStartOfPreparation = Time.fixedTime;
        StartCoroutine(StartTimers());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInfo.currentDayScore >= scoreThresholds[_numStarsActive])
        {
            _numStarsActive++;
            UpdateStarsState();
        }
    }


    public void UpdateStarsState()
    {
        for (int i = 0; i < _numStarsActive; i++)
        {
            starsAnimators[i].gameObject.SetActive(true);
        }
    }

    public void ResetStars()
    {
        for (int i = 0; i < starsAnimators.Length; i++)
        {
            starsAnimators[i].ResetMotion();
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

        canSpawnZombies = true;
        timerUI.color = dayTimerColor;
        _timeStartOfDay = _currentTime;
        while (_currentTime <= _timeStartOfDay + dayDuration)
        {
            ToTimeFormat(_timeStartOfDay + dayDuration - _currentTime);
            _currentTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        dayNeedsToEnd = true;
    }

    private void ToTimeFormat(float time)
    {
        _roundedTime = Mathf.RoundToInt(time);
        _seconds = _roundedTime % 60;
        _minutes = _roundedTime / 60;
        _secondsString = _seconds < 10 ? "0" + _seconds.ToString() : _seconds.ToString();
        _minutesString = _minutes < 10 ? "0" + _minutes.ToString() : _minutes.ToString();
        timerUI.text = _minutesString + ":" + _secondsString;
    }

    public void ToMainSlide()
    {
        screenAnimator.SetTrigger(ScreenAnimations.ToMainSlide);
    }

    public void ToSecondarySlide()
    {
        screenAnimator.SetTrigger(ScreenAnimations.ToSecondarySlide);
    }
}