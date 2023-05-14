using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICook
{
    public void Cook(string toolIdentifier);
    public IEnumerator StartCooking(string toolIdentifier);

    public void StopCooking();
}