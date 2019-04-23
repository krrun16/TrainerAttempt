using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Audio;
using System;

public class SphereScript : MonoBehaviour
{
    public float duration = 1; // in seconds
    private float startTime;
    private Vector3 beginPoint = new Vector3(5, 0, 0);
    private Vector3 finalPoint = new Vector3(5, 0, 0);
    private Vector3 farPoint = new Vector3(0, 0, 0);
    private int c;
    private GameObjectRecorder new_recorder;
    public bool record = false;
    public AudioSource audioData;
    private Rigidbody rb;
    public AudioMixerSnapshot farSideSnap;
    public AudioMixerSnapshot closeSideSnap;
    private const float maxspeed = 250;
    //public int startingPitch = 4;


    void Start()
    {
        new_recorder = new GameObjectRecorder(gameObject);
        new_recorder.BindComponentsOfType<Transform>(gameObject, true);
        rb = GetComponent<Rigidbody>();

        audioData = GetComponent<AudioSource>();
        //audioData.pitch = startingPitch;

        // audioData.Play(0);

        startTime = Time.time;
            beginPoint = new Vector3(5, 0, UnityEngine.Random.Range(30, 60));
    }




    private void FixedUpdate()
    {
        audioData.pitch = 0.35f;
        if (!audioData.isPlaying)
        {
            audioData.Play();
        }
        DynamicAudioChanges();
    }
    void Update()
    {

        Vector3 center = (beginPoint + finalPoint) * 0.5F;
        center -= farPoint;
        Vector3 riseRelCenter = beginPoint - center;
        Vector3 setRelCenter = finalPoint - center;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, (Time.time - startTime) / duration);
        transform.position += center;

    }
    public static float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        return (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
    }

    private void DynamicAudioChanges()
    {

        audioData.pitch = Scale(0, maxspeed, 0.8f, 1.25f, Math.Abs(rb.velocity.magnitude));
        //Dynamic LowPass Audio filter snapshot changing when ball passes halfway point
        if (rb.position.z > 0)
        {
            farSideSnap.TransitionTo(0.1f);
            Debug.Log("farside");
        }
        else
        {
            closeSideSnap.TransitionTo(0.1f);
            Debug.Log("here");
        }

        //Change Stereo Pan in buckets
        if (rb.position.z < -30)
        {
            audioData.panStereo = -1;
            Debug.Log("stereopan");
        }
        else if (rb.position.z >= -30 && rb.position.z < -10)
        {
            audioData.panStereo = -0.5f;
            Debug.Log(rb.position.x);
        }
        else if (rb.position.z >= -10 && rb.position.z < 10)
        {
            audioData.panStereo = 0;
            Debug.Log(rb.position.x);
        }
        else if (rb.position.z >= 10 && rb.position.z < 30)
        {
            audioData.panStereo = 0.5f;
            Debug.Log(rb.position.x);
        }
        else if (rb.position.z >= 30)
        {
            audioData.panStereo = 1;
            Debug.Log(rb.position.x);
        }
    }

    void OnDisable()
    {
        new_recorder.ResetRecording();

        //Debug.Log("End Position" + gameObject.transform.position);
    }
}







