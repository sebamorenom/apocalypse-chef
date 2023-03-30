using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    private string _gObjectName;

    // Start is called before the first frame update
    void Start()
    {
        _gObjectName = gameObject.name;
    }

    // Update is called once per frame
    public object SaveState()
    {
        Dictionary<string, object> dictSave = new Dictionary<string, object>();
        foreach (ISaveable savComponent in GetComponents<ISaveable>())
        {
            dictSave[savComponent.GetType().ToString()] = savComponent.CaptureState();
        }

        return dictSave;
    }

    public void LoadState(object state)
    {
        Dictionary<string, object> dictLoad = (Dictionary<string, object>)state;
        foreach (ISaveable savComponent in GetComponents<ISaveable>())
        {
            string typeString = savComponent.GetType().ToString();
            if (dictLoad.ContainsKey(typeString))
            {
                savComponent.LoadState(dictLoad[typeString]);
            }
        }
    }
}