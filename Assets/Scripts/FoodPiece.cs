using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPiece : MonoBehaviour
{
	public int id;
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		UpdateID();
	}
	
	public void UpdateID()
	{
		id=transform.GetSiblingIndex();	
	}
}
