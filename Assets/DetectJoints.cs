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
    private GameObjectRecorder new_recorder;
    public AnimationClip new_clip;
    public bool record = false;

    // Start is called before the first frame update
    void Start()
    {
        new_recorder = new GameObjectRecorder(gameObject);
        new_recorder.BindComponentsOfType<Transform>(gameObject, true);
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
                //Debug.Log("Start Position" + count + gameObject.transform.position);
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z);
                //Debug.Log("End Position" + count + gameObject.transform.position);
                count++;
            }
        }
    }

    void LateUpdate()
    {
        new_recorder.TakeSnapshot(Time.deltaTime);    
    }
    void OnDisable()
    {
        new_recorder.SaveToClip(new_clip);
        new_recorder.ResetRecording();

        //Debug.Log("End Position" + gameObject.transform.position);
    }
}
