using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateManager : MonoBehaviour
{
    /// <summary>
    /// please leave empty
    /// first point of the current plates path
    /// </summary>
    public GameObject firstPoint;
    /// <summary>
    /// on the main camera
    /// </summary>
    public ViewController viewController;
    /// <summary>
    /// child of the main camers
    /// </summary>
    public Transform camDirection;

    /// <summary>
    /// name display prefab
    /// </summary>
    public GameObject NameDisplay;
    /// <summary>
    /// child center of the HOS
    /// </summary>
    public GameObject center;
    /// <summary>
    /// home gameobject
    /// </summary>
    public GameObject home;
    /// <summary>
    /// child of the center gameobject in the HOS
    /// </summary>
    public GameObject paths;

    bool bSetPathAndMove = false;
    Transform currentPlateTrans;

    /// <summary>
    /// used for debugging when searching for plates
    /// </summary>
    /// <param name="plateId"></param>
    public void SearchForPlate(string plateId)
    {
        //Search name register for plate name
        
        if (plateId != "")
        {
            string region = plateId.Substring(0, 2);
            GameObject[] findPlates = GameObject.FindGameObjectsWithTag(region);
            List<GameObject> filterPlates = findPlates.ToList();
            foreach (var item in findPlates)
            {
                if (item.layer != this.gameObject.layer)
                {
                    filterPlates.Remove(item);
                }
            }
            
            string i = int.Parse(plateId.Substring(2)).ToString();

            GameObject findPlate = Array.Find(filterPlates.ToArray(), element => element.transform.name.Trim() == i);
            
            if (findPlate != null)
            {
                Plate plate = findPlate.GetComponent<Plate>();
                if (plate != null)
                {
                    plate.ThisPlateID = plateId;
                    viewController.plate = findPlate;
                    viewController.removeOldPlate();
                    plate.CreatUserNameDisplayAndLookAtIt(NameDisplay);
                    currentPlateTrans = plate.CameraPose;
                    plate.lastPathPoint = closestPoint(findPlate);
                    movePath(plate);
                    viewControllerManager(plate);


                }
                else
                {
                    plate = findPlate.AddComponent<Plate>();
                    plate.ThisPlateID = plateId;
                    viewController.plate = findPlate;
                    viewController.removeOldPlate();
                    plate.CreatUserNameDisplayAndLookAtIt(NameDisplay);
                    currentPlateTrans = plate.CameraPose;
                    plate.lastPathPoint = closestPoint(findPlate);
                    movePath(plate);
                    viewControllerManager(plate);

                }

            }
        }
        else 
        {
            Debug.Log("No such plate as: " + plateId);
        }




    }

    /// <summary>
    /// calculates the camera path and how to travel along it
    /// </summary>
    /// <param name="plate"></param>
    private void viewControllerManager(Plate plate)
    {
        //how long each rot/pos transform time should be 
        viewController.JourneyTimeForOnePoint = viewController.journeyTime;//viewController.journeyTime / (plate.lastPathPoint.GetComponent<PathPoint>().pointID + 1);
       // viewController.RotationTimeForOnePoint = viewController.rotateTime / (plate.lastPathPoint.GetComponent<PathPoint>().pointID + 1);
        
        //what path to go down for that plate
        viewController.Path = thePath(plate.lastPathPoint);
        viewController.CurrentDestPlate = plate;

        //has begun
        viewController.start = true;

    }
 
    private void movePath(Plate p)
    {
        if (p.lastPathPoint != null)
        {
            //sets the point nearest point to the plate as the last on the path
            GameObject point = p.lastPathPoint;

            //on that path sets the order of points
            bool pathEnd = false;
            while (pathEnd != true)
            {
                if (point.GetComponent<PathPoint>().prevPathPoint != null)
                {
                    point = point.GetComponent<PathPoint>().prevPathPoint;
                }
                else pathEnd = true;

            }
            firstPoint = point;

        }

    }

    /// <summary>
    /// gets all of the path points under the paths gameobject
    /// </summary>
    /// <returns></returns>
    private List<GameObject> pathPoints()
    {
        List<GameObject> list = new List<GameObject>();
        List<Transform> tList = paths.GetComponentsInChildren<Transform>().ToList();
        foreach(Transform t in tList)
        {
            list.Add(t.gameObject);

        }
        if (list.Contains(paths))
        {
            list.Remove(paths);
        }
        return list;
    }

    /// <summary>
    /// finds the closest point to the plate
    /// </summary>
    /// <param name="plateGameObject"></param>
    /// <returns></returns>
    private GameObject closestPoint(GameObject plateGameObject)
    {
        GameObject closest = gameObject;
        foreach(GameObject g in pathPoints())
        {
            if (closest == gameObject)
            {
                closest = g;
            }
            else if(Vector3.Distance(plateGameObject.transform.position,closest.transform.position) >= Vector3.Distance(plateGameObject.transform.position, g.transform.position))
            {
                closest = g;
            }
        }
        return closest;

    }

    /// <summary>
    /// generates a dictionary of all the points on a path and an ID for where thaey lie on that path
    /// </summary>
    /// <param name="lastPathPoint">closest point to the plate</param>
    /// <returns></returns>
    private Dictionary<int,GameObject> thePath(GameObject lastPathPoint)
    {
        Dictionary<int, GameObject> keyValuePairs = new Dictionary<int, GameObject>();

        keyValuePairs.Add(lastPathPoint.GetComponent<PathPoint>().pointID, lastPathPoint);
        bool pathEnded = false;
        int currentID = lastPathPoint.GetComponent<PathPoint>().pointID;
        while (pathEnded != true)
        {
            if (keyValuePairs[currentID].GetComponent<PathPoint>().prevPathPoint != null)
            {
                keyValuePairs.Add(keyValuePairs[currentID].GetComponent<PathPoint>().prevPathPoint.GetComponent<PathPoint>().pointID, keyValuePairs[currentID].GetComponent<PathPoint>().prevPathPoint);
                currentID--;
            }
            else pathEnded = true;
        }
        return keyValuePairs;
    }



}
