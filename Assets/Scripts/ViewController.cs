using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ViewController : MonoBehaviour {

    private float translationStartTime;
    private float rotationStartTime;

    Vector3 cameraPosition=new Vector3(0,0,0);
    Vector3 destPosition=new Vector3(0,0,0);

    Quaternion cameraRotation=new Quaternion(0,0,0,0);
    Quaternion destRotation=new Quaternion(0,0,0,0);

    private Vector3 center = new Vector3(0,0,0);

    private Plate currentDestPlate;
    public Plate CurrentDestPlate
    {
        get
        {
            return currentDestPlate;
        }

        set
        {
            currentDestPlate = value;
        }
    }

    public Transform homeLocation;

    public float journeyTime = 1.0F;
    public float rotateTime = 0.5f;

    private float jTForOnePoint;
    public float JourneyTimeForOnePoint
    {
        get
        {
            return jTForOnePoint;
        }

        set
        {
            jTForOnePoint = value;
        }
    }

    private float rTForOnePoint;
    public float RotationTimeForOnePoint
    {
        get
        {
            return rTForOnePoint;
        }

        set
        {
            rTForOnePoint = value;
        }
    }
    private Dictionary<int, GameObject> path;
    public Dictionary<int, GameObject> Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    public GameObject plate { get; set; }
    public GameObject canvas;



    public bool start = false;
    public bool end = false;

    public Transform centerOfRotation;
    public float waitTime;

    public delegate void OnSuccess(string path);
    public delegate void OnCancel();

    private void Start()
    {
        SetCameraPosition();
        center = centerOfRotation.position;

        

    }

    /// <summary>
    /// if a plate has been visited befor this resets it
    /// </summary>
    public void removeOldPlate()
    {
        try
        {

            Canvas[] canvases = plate.GetComponentsInChildren<Canvas>();
            UnityEngine.UI.CanvasScaler[] canvasScalers = plate.GetComponentsInChildren<UnityEngine.UI.CanvasScaler>();
            foreach (Canvas c in canvases)
            {
                c.enabled = false;
            }
            foreach (UnityEngine.UI.CanvasScaler c in canvasScalers)
            {
                c.enabled = false;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("First Time At Name");

        }

    }

    private void SetCameraPosition()
    {
        cameraPosition = this.transform.position;
        destPosition = cameraPosition;
        cameraRotation = this.transform.rotation;
        destRotation = cameraRotation;
    }

    /// <summary>
    /// used to return to the home position
    /// </summary>
    /// <param name="newPose">home</param>
    public void MoveCamera(Transform newPose)
    {

        SetCameraPosition();

        destPosition = newPose.position;
        destRotation = newPose.rotation;

        translationStartTime = Time.time;
        rotationStartTime = translationStartTime + journeyTime-rotateTime;
    }

    /// <summary>
    /// used to navigate to each of the points
    /// </summary>
    /// <param name="newPose">point</param>
    public void MoveCameraToPoint(Transform newPose)
    {

        cameraPosition = this.transform.position;
        cameraRotation = this.transform.rotation;
        destPosition = newPose.position;
        destRotation = newPose.rotation;

        translationStartTime = Time.time;
    }

    void Update()
    {
        //if not at home turn off UI
        if (transform.position == homeLocation.position)
        {

            canvas.SetActive(true);
        }
        else
        {

            canvas.SetActive(false);

        }

    }




    int currentPoint = 1;
    public bool goingHome = false;

    void FixedUpdate()
    {
        float translationFracComplete = (Time.time - translationStartTime) * JourneyTimeForOnePoint;

        //start
        if (start == true)
        {
            currentPoint = 1;
            MoveCameraToPoint(path[currentPoint].transform);
            goingHome = false;
            start = false;
            return;
        }

        //move on to the next point
        if (translationFracComplete >= 1f && path.ContainsKey(currentPoint + 1))
        {
            currentPoint++;
            MoveCameraToPoint(path[currentPoint].transform);
            return;
        }
        //return to home position
        else if (goingHome == true)
        {
            transform.LookAt(center, Vector3.up);

            UpdatePosition(translationFracComplete);
            return;
        }
        //move to the final point on path
        else if (translationFracComplete >= 1f && !path.ContainsKey(currentPoint + 1) && end == false)
        {
            end = true;

            MoveCameraToPoint(plate.GetComponent<Plate>().CameraPose);

            return;
        }

        //after wait period return home
        if (translationFracComplete >= waitTime)
        {
            removeOldPlate();
            MoveCamera(homeLocation);
            goingHome = true;
            end = false;
            return;
        }
        //do movements
        else
        {
            if(end == true)
            {
                UpdateRotation(translationFracComplete);
            }

            else transform.LookAt(center, Vector3.up);

            UpdatePosition(translationFracComplete);
        }


    }


    private void UpdatePosition(float fracComplete)
    {
        Vector3 center = centerOfRotation.position;
        center -= new Vector3(-center.x, 0, 0);

        Vector3 riseRelCenter = cameraPosition - center;
        Vector3 setRelCenter = destPosition - center;

        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);

        transform.position += center;
    }

    private void UpdateRotation(float fracComplete)
    {
        transform.rotation = Quaternion.Lerp(cameraRotation, destRotation, fracComplete);
    }


}
