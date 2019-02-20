using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.IO;
using AudioSource = UnityEngine.AudioSource;

public class CustomGestureManager : MonoBehaviour
{
    VisualGestureBuilderDatabase _gestureDatabase;
    VisualGestureBuilderFrameSource _gestureFrameSource;
    VisualGestureBuilderFrameReader _gestureFrameReader;
    KinectSensor _kinect;
    //Gesture _joust;
    //Gesture _tap;
    //Gesture _overhand;
    Gesture _swing;
    string _swingname = "swing.gbd";

    //public AudioClip joustnoise;
    //public AudioClip tapnoise;
    //public AudioClip overhandnoise;
    public AudioClip swingnoise;
    AudioSource audiosource = null;
    public GameObject AttachedObject;
    public BodySourceManager bodyManager;
    public void SetTrackingId(ulong id)
    {
        _gestureFrameReader.IsPaused = false;
        _gestureFrameSource.TrackingId = id;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }

    // Use this for initialization
    void Start()
    {
        bodyManager = this.gameObject.GetComponent<BodySourceManager>();
        _gestureDatabase = VisualGestureBuilderDatabase.Create(Application.dataPath + "/swing.gbd");
        _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0);
        _swing = _gestureDatabase.AvailableGestures[0];
        Debug.Log(_gestureDatabase.AvailableGestures[0]);
        Debug.Log(_gestureFrameSource);
        _gestureFrameSource.AddGesture(_swing);
        foreach (var gesture in _gestureDatabase.AvailableGestures)
        {
            _gestureFrameSource.AddGesture(gesture);

            /*if (gesture.Name == "joust")
            {
                _joust = gesture;
                audiosource.PlayOneShot(joustnoise);
            }*/
            if (gesture.Name == "swing")
            {
                _swing = gesture;

                audiosource.PlayOneShot(swingnoise);
            }
            /* if (gesture.Name == "tap")
            {
                _tap = gesture;
                audiosource.PlayOneShot(tapnoise);
            }
            if (gesture.Name == "overhand")
            {
                _overhand = gesture;
                audiosource.PlayOneShot(overhandnoise);
            }*/
        }

        _gestureFrameReader = _gestureFrameSource.OpenReader();
        _gestureFrameReader.IsPaused = true;
    }

    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null && frame.DiscreteGestureResults != null)
            {
                if (AttachedObject == null)
                    return;

                DiscreteGestureResult result = null;

                if (frame.DiscreteGestureResults.Count > 0)
                    result = frame.DiscreteGestureResults[_swing];
                if (result == null)
                    return;

                if (result.Detected == true)
                {
                    var progressResult = frame.ContinuousGestureResults[_swing];
                    if (AttachedObject != null)
                    {
                        var prog = progressResult.Progress;
                        float scale = 0.5f + prog * 3.0f;
                        AttachedObject.transform.localScale = new Vector3(scale, scale, scale);

                    }
                }

            }
        }
    }
}