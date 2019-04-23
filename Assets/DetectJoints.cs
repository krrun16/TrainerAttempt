using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEditor.Animations;
using Microsoft.Kinect.VisualGestureBuilder;

public class DetectJoints : MonoBehaviour
{
    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public float multiplier = 10;


    // Start is called before the first frame update
    void Start()
    {

        if (BodySrcManager == null)
        {
            Debug.Log("Assign!");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
    }
    int count = 0;

    // Update is called once per frame
    void Update()

    {

        if (bodyManager == null)
        {
            return;
        } bodies = bodyManager.GetData();
        if(bodies == null)
        {
            return;
        }
        foreach(var body in bodies)
        {
            if(body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                Vector3 start;
                Vector3 end;
                start = gameObject.transform.position;
                //Debug.Log("Start Position" + count + gameObject.transform.position);
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z);
                //Debug.Log("End Position" + count + gameObject.transform.position);
                end = gameObject.transform.position;
                count++;

               
            }
        }
    }


}
