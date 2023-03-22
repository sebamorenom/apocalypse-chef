using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private Director director;
    [SerializeField] private Color fadeColor;

    private float _changePerTickFadeIn;
    private float _changePerTickFadeOut;

    // Start is called before the first frame update
    private void Start()
    {
        fadeColor = Color.black;
        fadeColor.a = 0;
        _changePerTickFadeIn = Time.fixedDeltaTime / fadeInTime;
        _changePerTickFadeOut = Time.fixedDeltaTime / fadeOutTime;
        director.startDay += StartFadeIn;
        director.endDay += StartFadeOut;
    }

    public void StartFadeIn()
    {
        fadeColor.a = 1;
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        fadeColor.a = 0;
        StartCoroutine(FadeOut());
    }

    public void StartFadeOutFadeIn()
    {
        fadeColor.a = 0;
        StartCoroutine(FadeOutFadeIn());
    }

    public IEnumerator FadeOutFadeIn()
    {
        yield return StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (fadeColor.a != 0)
        {
            fadeColor.a = Mathf.Max(fadeColor.a - (_changePerTickFadeIn * 2), 1);
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private IEnumerator FadeOut()
    {
        while (fadeColor.a != 1)
        {
            fadeColor.a = Mathf.Min(fadeColor.a + (_changePerTickFadeOut * 2), 1);
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}