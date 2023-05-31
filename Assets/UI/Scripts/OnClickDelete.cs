using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularMotion;

public class OnClickDelete : MonoBehaviour
{

    public GameObject ToDelete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DeleteOnClick()
    {
        GameObject.Destroy(ToDelete);
    }


}
