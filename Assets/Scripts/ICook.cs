using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICook
{
    public IEnumerator Cook(string toolIdentifier);
}