using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;
using System.IO;
using AudioSource = UnityEngine.AudioSource;
using System;


public class NewGesMan : MonoBehaviour
{
    public BodySourceManager bodyManager;
    private Body[] bodies;
    private readonly string JoustProgressName = "joustProgress";
    private readonly string SwingProgressName = "swingProgress";
    private readonly string OverhandProgressName = "forehandProgress";
    private readonly string Swingend = "finishswing";
    private readonly string Joustend = "joustend";
    private readonly string Forehandend = "forehandend";
    private readonly string Overheadend = "endabove";


    public AudioClip joustsound;
    public AudioClip swingsound;
    public AudioClip overhandsound;
    public AudioClip overheadsound;
    public AudioClip endsound;


    private AudioSource audioSource;
    private AudioSource audioSource1;
    VisualGestureBuilderFrameSource _vgbframesource = null;
    VisualGestureBuilderFrameReader vgbFrameReader;
    KinectSensor _kinect = null;
    // Use this for initialization
    void Start()
    {
        _kinect = KinectSensor.GetDefault();
        if (_kinect != null)
        {


            _vgbframesource = VisualGestureBuilderFrameSource.Create(_kinect, 0);
            vgbFrameReader = _vgbframesource.OpenReader();
            this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            audioSource = GetComponent<AudioSource>();
            audioSource1 = GetComponent<AudioSource>();

            if (vgbFrameReader != null)
            {
                vgbFrameReader.IsPaused = true;
            }
            var databasePath = @"Database\newtry.gbd";
            using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
            {

                foreach (Gesture gesture in database.AvailableGestures)
                {
                    this._vgbframesource.AddGesture(gesture);
                    Debug.Log(gesture.Name);


                }
            }
        }
    }

    private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            if (frame != null)
            {
                var continuousResults = frame.ContinuousGestureResults;
                var discreteResults = frame.DiscreteGestureResults;
                if (discreteResults != null)
                {
                    foreach (Gesture gesture in this._vgbframesource.Gestures)
                    {
                        if (gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                if (result.Detected != false)
                                    {
                                    //Debug.Log(gesture.Name);
                                    if (gesture.Name.Equals(Swingend))
                                        {

                                            audioSource.clip = swingsound;
                                            audioSource.Play();
                                    }
                                    if (gesture.Name.Equals(Joustend))
                                        {
                                            audioSource.clip = joustsound;
                                            audioSource.Play();
                                        }
                                        if (gesture.Name.Equals(Forehandend))
                                        {

                                                audioSource.clip = overhandsound;
                                                audioSource.Play();
                                            
                                        }
                                    if (gesture.Name.Equals(Overheadend))
                                    {

                                        audioSource.clip = overheadsound;
                                        audioSource.Play();

                                    }


                                }
                            }
                        }

                        if (continuousResults != null)
                        {

                            if (gesture.Name.Equals(this.SwingProgressName) && gesture.GestureType == GestureType.Continuous)
                            {
                                ContinuousGestureResult result = null;
                                continuousResults.TryGetValue(gesture, out result);
                                if (result.Progress >= .97)
                                {
                                    // Debug.Log(gesture.Name);


                                }

                            }
                            if (gesture.Name.Equals(this.JoustProgressName) && gesture.GestureType == GestureType.Continuous)
                            {

                                ContinuousGestureResult result = null;
                                continuousResults.TryGetValue(gesture, out result);
                                if (result.Progress == 1)
                                {
                                    Debug.Log(result.Progress + gesture.Name);

                                }



                            }

                            if (gesture.Name.Equals(this.OverhandProgressName) && gesture.GestureType == GestureType.Continuous)
                            {
                                ContinuousGestureResult result = null;
                                continuousResults.TryGetValue(gesture, out result);
                                if (result.Progress == 1)
                                {
                                    Debug.Log(result.Progress + gesture.Name);

                                }

                            }
                        }
                        
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

            }
        }

        }


}







