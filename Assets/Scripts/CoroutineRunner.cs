using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    // Start is called before the first frame update
    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}