using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class Plate : MonoBehaviour
{
    #region PrivateFields
    private Mesh plateMesh;

    private Vector3 normalVector;

    private Transform cameraPose;
    private Transform cameraPoseModified;
    public Transform CameraPose
    { get { return cameraPoseModified; } }


    private Transform namePose;
    public Transform NamePose
    { get { return namePose; } }

    private string thisPlateID;
    public string ThisPlateID
    {
        get
        {
            return thisPlateID;
        }

        set
        {
            thisPlateID = value;
        }
    }

    private Transform cameraPosition;

    private float viewDistance = 0.5f;
    #endregion
    public GameObject nameObject;
    public bool badPlate;
    public float customZRotation = 0f;

    Vector3[] vertices;//plate vertices
    List<Vector3> corners = new List<Vector3>();//the 8 corners of the plate
    GameObject verts;
    GameObject vert;
    List<GameObject[]> myCorners = new List<GameObject[]>();
    Dictionary<string, GameObject> labledCorners = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> backLabledCorners = new Dictionary<string, GameObject>();
    List<Vector3[]> pairs = new List<Vector3[]>();
    GameObject verticieRotHolder;
    GameObject parent;
    public GameObject lastPathPoint;
    Dictionary<string, GameObject> innerBoundPoints = new Dictionary<string, GameObject>();
    GameObject cameraSight;

    void Awake()
    {
        //empty prefab
        vert = GameObject.Find("Vert Template");

        plateMesh = GetComponent<MeshFilter>().mesh;

        //the plate and all its childrens transforms
        Transform[] plateTransforms = this.GetComponentsInChildren<Transform>();
        vertices = plateMesh.vertices;

        //if the plate has no children/no custom adjustments
        if (plateTransforms.Length == 1)
        {
            CalcNormals();

            SetCameraPosition();
        }
        else
        {
            foreach (Transform currentTransform in plateTransforms)
            {
                if (currentTransform != this.transform && currentTransform.name == "CameraPosition")
                {
                    cameraPose = currentTransform;

                    //camera sight object holds the pos/rot of the camera so that it can return there later
                    GameObject cameraSight = Instantiate(vert, currentTransform);
                    cameraSight.name = "Camera Sight";
                    cameraPoseModified = cameraSight.transform;
                }
                if (currentTransform != this.transform && currentTransform.name == "Name Display")
                {
                    namePose = currentTransform;
                }


            }
        }

        if (badPlate == true) return;

        FindCorners();
        GenerateCornerObjects();
        SortCorners();

    }
    /// <summary>
    /// calculats what corners are at the front of the plate 
    /// </summary>
    private void SortCorners()
    {
        Vector3 compareVect = verticieRotHolder.transform.localPosition;

        foreach (GameObject[] thisPair in myCorners)
        {
            thisPair[0].transform.parent = verticieRotHolder.transform;
            thisPair[1].transform.parent = verticieRotHolder.transform;
        }

        GameObject.Destroy(parent);
        foreach (GameObject[] thisPair in myCorners)
        {
            int front;
            int back;
            float dist0 = Vector3.Distance(cameraPose.position, thisPair[0].transform.position);
            float dist1 = Vector3.Distance(cameraPose.position, thisPair[1].transform.position);
            if (dist0 <= dist1)
            {
                thisPair[0].name = "Front";
                thisPair[1].name = "Back";

                thisPair[0].transform.forward = ((thisPair[0].transform.localPosition - thisPair[1].transform.localPosition).normalized);

                front = 0;
                back = 1;
            }
            else
            {
                thisPair[1].name = "Front";
                thisPair[0].name = "Back";

                thisPair[1].transform.forward = ((thisPair[1].transform.localPosition - thisPair[0].transform.localPosition).normalized);

                front = 1;
                back = 0;
            }



            if (compareVect.x > thisPair[front].transform.localPosition.x && compareVect.y > thisPair[front].transform.localPosition.y)
            {
                thisPair[front].name = "Bottem Left";
                thisPair[back].name = "Back Bottem Left";
            }
            else if (compareVect.x < thisPair[front].transform.localPosition.x && compareVect.y > thisPair[front].transform.localPosition.y)
            {
                thisPair[front].name = "Bottem Right";
                thisPair[back].name = "Back Bottem Right";
            }
            else if (compareVect.x > thisPair[front].transform.localPosition.x && compareVect.y < thisPair[front].transform.localPosition.y)
            {
                thisPair[front].name = "Top Left";
                thisPair[back].name = "Back Top Left";
            }
            else if (compareVect.x < thisPair[front].transform.localPosition.x && compareVect.y < thisPair[front].transform.localPosition.y)
            {
                thisPair[front].name = "Top Right";
                thisPair[back].name = "Back Top Right";
            }

            
            try
            {
                labledCorners.Add(thisPair[front].name, thisPair[front]);
                backLabledCorners.Add(thisPair[back].name, thisPair[back]);
            }
            catch (Exception AlreadyACorner)
            {

                Debug.Log(thisPair[front].transform.parent.parent.parent.parent.name + thisPair[front].transform.parent.parent.parent.name + "\t" + AlreadyACorner);
            }

        }


        labledCorners["Bottem Left"].transform.LookAt(backLabledCorners["Back Bottem Left"].transform, (labledCorners["Top Left"].transform.position - labledCorners["Bottem Left"].transform.position).normalized);
        labledCorners["Top Left"].transform.LookAt(backLabledCorners["Back Top Left"].transform, (labledCorners["Top Left"].transform.position - labledCorners["Bottem Left"].transform.position).normalized);
        labledCorners["Bottem Right"].transform.LookAt(backLabledCorners["Back Bottem Right"].transform, (labledCorners["Top Right"].transform.position - labledCorners["Bottem Right"].transform.position).normalized);
        labledCorners["Top Right"].transform.LookAt(backLabledCorners["Back Top Right"].transform, (labledCorners["Top Right"].transform.position - labledCorners["Bottem Right"].transform.position).normalized);
        foreach (GameObject obj in backLabledCorners.Values)
        {
            Destroy(obj);
        }





    }
    /// <summary>
    /// findt the 8 corners of a plate
    /// </summary>
    private void FindCorners()
    {
        verts = Instantiate(vert, transform);
        verts.name = "Vertices";
        verticieRotHolder = Instantiate(vert, verts.transform);
        verticieRotHolder.name = "verticieRotHolder";
        verticieRotHolder.transform.localRotation = cameraPose.localRotation;

        foreach (Vector3 vect in vertices)
        {
            if (corners.Find(corn => corn.magnitude < vect.magnitude) != null)
            {
                if (!corners.Contains(vect))
                {
                    corners.Add(vect);
                    if (corners.Count == 9)
                    {
                        corners = corners.OrderBy(v => v.magnitude).ToList();
                        corners.RemoveAt(0);
                    }
                }
            }
        }




        for (int x = 0; x < 4; x++)
        {
            Vector3 otherCorner = corners.Find(cornerP => Vector3.Distance(cornerP, corners[0]) <= 0.05f && Vector3.Distance(cornerP, corners[0]) >= -0.05f && cornerP != corners[0] && Vector3.Distance(cornerP, corners[0]) != 0);
            Vector3[] pair = new Vector3[2] { corners[0], otherCorner };

            pairs.Add(pair);
            corners.RemoveAt(0);
            corners.Remove(otherCorner);

        }

    }

    /// <summary>
    /// generates thos corners as objects 
    /// </summary>
    private void GenerateCornerObjects()
    {

        parent = Instantiate(vert, verts.transform);
        parent.name = "Parent";
        for (int x = 0; x < 4; x++)
        {
            GameObject point = Instantiate(vert, parent.transform);
            point.transform.localPosition = pairs[x][0];
            point.transform.localRotation = verticieRotHolder.transform.localRotation;
            point.name = "firstCorner " + x;

            GameObject point2 = Instantiate(vert, parent.transform);
            point2.transform.localPosition = pairs[x][1];
            point2.transform.localRotation = verticieRotHolder.transform.localRotation;
            point2.name = "secondCorner " + x;
            myCorners.Add(new GameObject[2] { point, point2 });


        }
    }

    private void CalcNormals()
    {
        Vector3[] normals = plateMesh.normals;
        Vector3 sum = new Vector3(0, 0, 0);

        foreach (Vector3 normal in normals)
        {
            sum += normal;
        }

        normalVector = sum / normals.Length;
    }

    private void SetCameraPosition()
    {
        GameObject normal = new GameObject();
        normal.name = "CameraPosition";
        normal.transform.parent = this.transform;

        normal.transform.localPosition = normalVector;

        normal.transform.LookAt(this.transform.position);

        normal.transform.Translate(Vector3.forward * viewDistance);

        normal.transform.LookAt(this.transform.position);

        cameraPose = normal.transform;


        cameraSight = Instantiate(vert, normal.transform);
        cameraSight.name = "Camera Sight";
        cameraPoseModified = cameraSight.transform;

    }


    public void CreatUserNameDisplayAndLookAtIt(GameObject nameDisplay)
    {
        DatabaseLookup lookup = GetComponentInParent<DatabaseLookup>();//gets the current database

        cameraPoseModified.position = cameraPose.position; //returns the cameraPose to original position after calculations

        #region name display instance manager
        if (badPlate == true)
        {
            nameObject = namePose.gameObject;
        }
        else
        {
            //removes previos plate name object
            try
            {
                GameObject oldName = transform.Find("Name Display").gameObject;
                Destroy(oldName);
            }
            catch { }

            nameObject = Instantiate(nameDisplay, transform.GetChild(0).gameObject.transform);
            nameObject.name = "Name Display";
        }
        #endregion

        //sets up ui elements
        nameObject.GetComponent<Canvas>().enabled = true;
        nameObject.GetComponent<CanvasScaler>().enabled = true;

        //will only catch if searching for plate id
        try
        {
            nameObject.GetComponentInChildren<Text>().text = lookup.LastPlate.GetName();
        }
        catch
        {
            nameObject.GetComponentInChildren<Text>().text = "Debug";
        }

        //the rest has a custom set up
        if (badPlate == true)
        {
            return;
        }


        float numberOfRows, numbeOfColumns, rowPercent, columnPercent;
        //calculate the numerecal bounds of the engraved names on the plate and where this name lies in that
        try
        {
            numberOfRows = lookup.MaxColumnRows[lookup.LastPlate.GetPanel()].maxRow + 1;
            numbeOfColumns = lookup.MaxColumnRows[lookup.LastPlate.GetPanel()].maxColumn + 1;
            rowPercent = ((1 / numberOfRows) * (lookup.LastPlate.GetRow()));
            columnPercent = ((1 / numbeOfColumns) * (lookup.LastPlate.GetColumn()));
        }
        catch (Exception error)
        {
            //if something goes wron stick the name in the center of the plate
            numberOfRows = 11;
            numbeOfColumns = 11;
            rowPercent = ((1 / numberOfRows) * 6);
            columnPercent = ((1 / numbeOfColumns) * 6);
            Debug.LogWarning("No Such Name on Any Plate \n" + error);

        }

        generateInnerBoundPoints();


        //calculates a grid within the inner bounds
        Vector3 row = Vector3.Lerp(innerBoundPoints["Bottem Left"].transform.localPosition, innerBoundPoints["Bottem Right"].transform.localPosition, 1 - rowPercent);
        Quaternion rowQ = Quaternion.Lerp(innerBoundPoints["Bottem Left"].transform.localRotation, innerBoundPoints["Bottem Right"].transform.localRotation, 1- rowPercent);
        Vector3 colomn = Vector3.Lerp(innerBoundPoints["Bottem Left"].transform.localPosition, innerBoundPoints["Top Left"].transform.localPosition, columnPercent);
        Quaternion columnQ = Quaternion.Lerp(innerBoundPoints["Bottem Left"].transform.localRotation, innerBoundPoints["Top Left"].transform.localRotation, columnPercent);



        //where the name should go on the inner grid
        Vector3 gridLocation = row + colomn - innerBoundPoints["Bottem Left"].transform.localPosition;
        Vector3 gridPoint = verticieRotHolder.transform.TransformPoint(gridLocation);

        //raycast where the name display should go
        RaycastHit hit;
        Physics.Raycast(nameObject.transform.position, gridPoint - nameObject.transform.position, out hit);

        // set the name displays position/rotation
        nameObject.transform.localPosition = nameObject.transform.parent.InverseTransformPoint(hit.point) - new Vector3(0, 0, 0.007f);
        nameObject.transform.localRotation = Quaternion.Inverse(innerBoundPoints["Bottem Left"].transform.localRotation) * rowQ * columnQ;
        nameObject.transform.Rotate(new Vector3(0, 0, customZRotation), Space.Self);
        nameObject.transform.parent = transform;

        //make the camera look at the name display
        cameraPoseModified.LookAt(nameObject.transform, nameObject.transform.up);


    }

    /// <summary>
    /// if the surface of the plate is a grid. then this method calculates the four corners of a grid within the plate grid based on a margin bound
    /// </summary>
    void generateInnerBoundPoints()
    {
        float bound = 0.35f;//the margin bound

        //sets up the corners for itterative calculations
        Dictionary<int,KeyValuePair<string, Vector2>> boundMod = new Dictionary<int, KeyValuePair<string, Vector2>>();
        boundMod.Add(0, new KeyValuePair<string, Vector2>("Bottem Left", new Vector2(0 + bound, 0 + bound)));
        boundMod.Add(1, new KeyValuePair<string, Vector2>("Top Left", new Vector2(0 + bound, 1 - bound)));
        boundMod.Add(2, new KeyValuePair<string, Vector2>("Top Right", new Vector2(1 - bound, 1 - bound)));
        boundMod.Add(3, new KeyValuePair<string, Vector2>("Bottem Right", new Vector2(1 - bound, 0 + bound)));

        for (int i = 0; i < 4; i++)
        {
            if (innerBoundPoints.Count == 4)//just in case it loops wrong
            {
                return;
            }
            Vector3 rowInnerGrid = Vector3.Lerp(labledCorners["Bottem Left"].transform.localPosition, labledCorners["Bottem Right"].transform.localPosition, boundMod[i].Value.y);
            Vector3 colomnInnerGrid = Vector3.Lerp(labledCorners["Bottem Left"].transform.localPosition, labledCorners["Top Left"].transform.localPosition, boundMod[i].Value.x);

            Quaternion rowQuaternionInnerGrid = Quaternion.Lerp(labledCorners["Bottem Left"].transform.localRotation, labledCorners["Bottem Right"].transform.localRotation, boundMod[i].Value.y);
            Quaternion columnQuaternionInnerGrid = Quaternion.Lerp(labledCorners["Bottem Left"].transform.localRotation, labledCorners["Top Left"].transform.localRotation, boundMod[i].Value.x);

            //calculates local positions to each other. ignores world space 
            Vector3 gridLocationIG = rowInnerGrid + colomnInnerGrid - labledCorners["Bottem Left"].transform.localPosition;
            Vector3 gridRotationIG =  (Quaternion.Inverse(labledCorners["Bottem Left"].transform.localRotation) * rowQuaternionInnerGrid * columnQuaternionInnerGrid).eulerAngles;

            //creat gameobject for each inner bound
            GameObject innerVert =Instantiate(vert, labledCorners["Bottem Left"].transform.parent);
            innerVert.transform.localPosition = gridLocationIG;
            innerVert.transform.localRotation = Quaternion.Inverse(labledCorners["Bottem Left"].transform.localRotation) * rowQuaternionInnerGrid * columnQuaternionInnerGrid;
            innerVert.name = "Inner " + boundMod[i].Key;
            innerBoundPoints.Add(boundMod[i].Key, innerVert);


        }

    }

}
