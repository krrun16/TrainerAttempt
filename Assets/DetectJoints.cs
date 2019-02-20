using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DetectJoints : MonoBehaviour
{
    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public float multiplier = 15;
    static readonly float MaxRecordingTime = 5000.0f;
    float m_stopRecordingTimer = float.MaxValue;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Position" + gameObject.transform.position);

        if (BodySrcManager == null)
        {
            Debug.Log("Assign!");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
    }

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
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z);
            }
        }
    }
    void OnDisable()
    {
        Debug.Log("End Position" + gameObject.transform.position);
    }
}
