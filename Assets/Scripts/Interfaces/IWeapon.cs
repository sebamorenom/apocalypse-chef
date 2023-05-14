using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeapon
{
    public void OnHit();
    public void OnThrow();
    public IEnumerator OnHitTimer();
    public IEnumerator OnThrowTimer();
}