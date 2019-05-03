using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTheGameObject : MonoBehaviour {

    bool prevState;
    public GameObject obj;

    public void ToggleObjNow()
    {

        if (obj.activeSelf == true)
        {
            obj.SetActive(false);

        }
        else
        {

            obj.SetActive(true);

        }
    }
}
