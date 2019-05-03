using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script that maps the points of the paths to each other
/// </summary>
public class PathPoint : MonoBehaviour {
    public GameObject prevPathPoint;
    public GameObject nextPathPoint;

    public int pointID = 404;//point not found




    // Use this for initialization
    void Start () {
        if (prevPathPoint != null)
        {
            prevPathPoint.GetComponent<PathPoint>().nextPathPoint = gameObject;
            transform.LookAt(gameObject.transform.parent.parent, Vector3.up);
        }
        else pointID = 0;

	}
	
	// Update is called once per frame
	void Update () {
        if (pointID >= 404 && prevPathPoint != null)
        {
            pointID = prevPathPoint.GetComponent<PathPoint>().pointID + 1;
        }
		
	}
}
