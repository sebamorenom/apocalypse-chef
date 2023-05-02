using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Fader))]
public class SceneChanger : MonoBehaviour
{
    [HideInInspector] public static SceneChanger scenechanger;
    private Fader _fader;

    private void Awake()
    {
        _fader = GetComponent<Fader>();
    }

    public void ChangeScene(int sceneIndex)
    {
        _fader.StartFadeOut();
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(WaitUntilSceneLoaded(sceneIndex));
        _fader.StartFadeIn();
    }

    private IEnumerator WaitUntilSceneLoaded(int sceneIndex)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneIndex)
        {
            yield return null;
        }
    }
}