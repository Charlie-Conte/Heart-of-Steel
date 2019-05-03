using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to cycle through all the plates in each section of the heart seperatly
/// </summary>
public class PlateDebugger : MonoBehaviour {

    public PlateManager plateManager;
    private int plateCounter = 1;
    private float timeStep = 0;

    private int timer = 5;
    private void Start()
    {
        GameObject.Find("Main Camera").GetComponent<ViewController>().journeyTime = 2;
    }
    // Update is called once per frame
    void Update () {
        if (Time.time-timeStep>timer)
        {
            if (plateCounter >= 40) return;
            plateManager.SearchForPlate(this.name + plateCounter.ToString("000"));
            //Debug.Log(this.name + plateCounter.ToString("000"));
            plateCounter++;
            timeStep = Time.time;
        }        
	}
}
