using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;
using System.IO;

public class NewGesMan : MonoBehaviour
{
    Gesture swing;
    string gestureName = "swing";
    VisualGestureBuilderFrameSource _vgbframesource = null;
    VisualGestureBuilderFrameReader vgbFrameReader = null;
    KinectSensor _kinect = null;

    // Use this for initialization
    void Start()
    {
        _kinect = KinectSensor.GetDefault();
        if (_kinect != null)
        {


            _vgbframesource = VisualGestureBuilderFrameSource.Create(_kinect, 0);
            vgbFrameReader = _vgbframesource.OpenReader();

            //            if (vgbFrameReader != null)
            //            {
            //                vgbFrameReader.IsPaused = true;
            //                Debug.Log("vgbFrameReader is paused");
            //            }

            var databasePath = Path.Combine(Application.dataPath, "swing.gbd");
            using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
            {

                foreach (Gesture gesture in database.AvailableGestures)
                {

                    if (gesture.Name.Equals(gestureName))
                    {
                        this._vgbframesource.AddGesture(gesture);
                    }


                }
            }
        }
    }
}

   