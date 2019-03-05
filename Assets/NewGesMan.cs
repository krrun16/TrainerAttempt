using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;
using System.IO;
using AudioSource = UnityEngine.AudioSource;

public class NewGesMan : MonoBehaviour
{
    public BodySourceManager bodyManager;
    private Body[] bodies;

    //public AudioClip swingnoise;
    //public AudioClip joustnoise;
    //public AudioClip overhandnoise;
    //public AudioClip tapnoise;
    //AudioSource audiosource;
    VisualGestureBuilderFrameSource _vgbframesource = null;
    VisualGestureBuilderFrameReader vgbFrameReader;
    KinectSensor _kinect = null;
    // Use this for initialization
    void Start()
    {
        _kinect = KinectSensor.GetDefault();
        if (_kinect != null)
        {
            //audiosource = GetComponent<AudioSource>();


            _vgbframesource = VisualGestureBuilderFrameSource.Create(_kinect, 0);
            vgbFrameReader = _vgbframesource.OpenReader();

            if (vgbFrameReader != null)
            {
                vgbFrameReader.IsPaused = true;
            }
            var databasePath = @"Database\swingset.gbd";
            using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
            {

                foreach (Gesture gesture in database.AvailableGestures)
                {
                    this._vgbframesource.AddGesture(gesture);

                }
            }
        }
    }



    void Update()
    {
        if (bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();
        if (bodies == null)
        {
            return;
        }
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                _vgbframesource.TrackingId = body.TrackingId;
                vgbFrameReader.IsPaused = false;
                if (vgbFrameReader != null)
                {
                    vgbFrameArrived();
                }
            }
        }
    }

    public void vgbFrameArrived()
    {
        using (var frame = this.vgbFrameReader.CalculateAndAcquireLatestFrame())
        {
            if (frame != null)
            {
                foreach (var result in frame.DiscreteGestureResults)
                {
                    Gesture gesture = result.Key;
                    var gestureResult = result.Value as DiscreteGestureResult;
                    float lastConfidence = gestureResult.Confidence;
                    if (lastConfidence > 0)
                    {
                        Debug.Log(lastConfidence);
                        //Debug.Log(gesture.Name);
                    }
                }
                foreach (var result in frame.ContinuousGestureResults)
                {
                    Gesture gesture = result.Key;
                    var gestureResult = result.Value as ContinuousGestureResult;
                    float lastProgress = gestureResult.Progress;

                }
            }
        }
    }
}
    
    
    
    /*
    public void UpdateGesture()
    {

        using (var frame = this.vgbFrameReader.CalculateAndAcquireLatestFrame())
        {

            if (frame != null)
            {
                var discreteGes = frame.DiscreteGestureResults;
                var continuousGes = frame.ContinuousGestureResults;
                if (discreteGes != null)
                {

                   foreach (var gesture in this._vgbframesource.Gestures)




                        if (gesture.GestureType == GestureType.Discrete)
                        {
                                    if (gesture.Name.Equals(this.swing))
                                    {
                                        //audiosource.PlayOneShot(swingnoise, .7f);
                                        //Debug.Log("Great swing!");

                                    }
                                    else if(gesture.Name.Equals(this.joust))
                                    {
                                        //Debug.Log("Bad swing");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        
    

                                
                                else if (gesture.Name.Equals("joust"))
                                {
                                    //audiosource.PlayOneShot(joustnoise, .7f);

                                    //Debug.Log("Joust swing!");

                                }
                                else if (gesture.Name.Equals("overhand"))
                                {
                                    //audiosource.PlayOneShot(overhandnoise, .7f);
                                    //Debug.Log("Overhand swing!");

                                }
                                else if (gesture.Name.Equals("tap"))
                                {
                                    //audiosource.PlayOneShot(tapnoise, .7f);
                                    //Debug.Log("Tap swing!");

                                }
                            }


                        }
                    }
                }
            }

        }
    }
}*/



   