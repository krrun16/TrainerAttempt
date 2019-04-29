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
    private Vector3 beginPoint = new Vector3(-5, 0, 0);
    private Vector3 finalPoint = new Vector3(5, 0, 0);
    private Vector3 farPoint = new Vector3(0, 0, 1);
    private int c;
    private GameObjectRecorder new_recorder;
    public bool record = false;
    public AudioSource audioData;
    private Rigidbody rb;




    void Start()
    {
        new_recorder = new GameObjectRecorder(gameObject);
        new_recorder.BindComponentsOfType<Transform>(gameObject, true);
        rb = GetComponent<Rigidbody>();

        audioData = GetComponent<AudioSource>();
        //audioData.pitch = startingPitch;

        // audioData.Play(0);

        startTime = Time.time;
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


}







