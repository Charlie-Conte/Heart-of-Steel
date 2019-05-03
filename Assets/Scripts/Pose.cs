using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose {

    private Vector3 rVec=new Vector3(0,0,0);
    public Vector3 Rotation
    { get { return rVec; } }

    private Vector3 tVec = new Vector3(0, 0, 0);
    public Vector3 Translation
    { get { return tVec; } }

    public Pose(){ }

    public Pose(Vector3 position, Vector3 rotation)
    {
        rVec = rotation;
        tVec = position;
    }
}
