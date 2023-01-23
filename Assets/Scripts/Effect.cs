using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    [SerializeField] public bool isExplosive;
    [SerializeField] public bool isSticky;
    [SerializeField] public bool isNoisy;
}