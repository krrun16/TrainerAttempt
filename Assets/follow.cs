using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Audio;
using System;

public class follow : MonoBehaviour
{

    public GameObject player;
    private int followDistance = 15000;
    private List<Vector3> storedPositions;
    public GameObject player2;
    public bool record = false;
    public AudioSource audioData;
    private Rigidbody rb;
    public AudioMixerSnapshot farSideSnap;
    public AudioMixerSnapshot closeSideSnap;


    void Start()
    {
        audioData = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

    }
    void Awake()
    {
        storedPositions = new List<Vector3>(); //create a blank list

    }

    void LateUpdate()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 player2Pos = player2.transform.position;

        if (storedPositions.Count == 0) //check if the list is empty
        {
            storedPositions.Add(playerPos); //store the players currect position
            Debug.Log("Player 1" + playerPos);

            player2.transform.position = playerPos; //move
            Debug.Log("Player 2" + player2Pos);

        }
        else if (storedPositions[storedPositions.Count - 1] != playerPos)
        {
            storedPositions.Add(playerPos); //store the position every frame
            Debug.Log(playerPos);
            player2.transform.position = new Vector3(playerPos.x, 2, playerPos.z); //move
            Debug.Log("Player 2" + player2Pos);
        }

        if (storedPositions.Count > followDistance)
        {
            player2.transform.position = storedPositions[0]; //move
            storedPositions.RemoveAt(0); //delete the position that player just move to
        }
    }
    public static float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        return (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
    }

    private void DynamicAudioChanges()
    {

        //Change and limit pitch change on ball
        audioData.pitch = Scale(0, 70, 0.8f, 1.25f, Math.Abs(rb.velocity.magnitude));

        //Change rolling sounds based on speed of ball
        //ballSoundSource.volume = GameUtils.Scale(0, maxspeed, 0, 1, Math.Abs(rb.velocity.magnitude));

        //Dynamic LowPass Audio filter snapshot changing when ball passes halfway point
        if (rb.position.z > 0)
        {
            farSideSnap.TransitionTo(0.1f);
            Debug.Log("Here");
        }
        else
        {
            closeSideSnap.TransitionTo(0.1f);
            Debug.Log("Here1");
        }

        //Change Stereo Pan in buckets
        if (rb.position.x < -30)
        {
            audioData.panStereo = -1;
            Debug.Log("Here2");
        }
        else if (rb.position.x >= -30 && rb.position.x < -10)
        {
            audioData.panStereo = -0.5f;
            Debug.Log("Here3");
        }
        else if (rb.position.x >= -10 && rb.position.x < 10)
        {
            audioData.panStereo = 0;
            Debug.Log("Here4");
        }
        else if (rb.position.x >= 10 && rb.position.x < 30)
        {
            audioData.panStereo = 0.5f;
            Debug.Log("Here5");
        }
        else if (rb.position.x >= 30)
        {
            audioData.panStereo = 1;
            Debug.Log("Here6");
        }
    }

    private void DynamicAudioChanges2()
    {

        //Change and limit pitch change on ball
        audioData.pitch = Scale(0, 70, 0.8f, 1.25f, Math.Abs(rb.velocity.magnitude));

        //Change rolling sounds based on speed of ball
        //ballSoundSource.volume = GameUtils.Scale(0, maxspeed, 0, 1, Math.Abs(rb.velocity.magnitude));

        //Dynamic LowPass Audio filter snapshot changing when ball passes halfway point
        if (rb.position.z > 0)
        {
            farSideSnap.TransitionTo(0.1f);
            Debug.Log("Here");
        }
        else
        {
            closeSideSnap.TransitionTo(0.1f);
            Debug.Log("Here1");
        }

        //Change Stereo Pan in buckets
        if (rb.position.z < -30)
        {
            audioData.panStereo = -1;
            Debug.Log("Here2");
        }
        else if (rb.position.z >= -30 && rb.position.z < -10)
        {
            audioData.panStereo = -0.5f;
            Debug.Log("Here3");
        }
        else if (rb.position.z >= -10 && rb.position.z < 10)
        {
            audioData.panStereo = 0;
            Debug.Log("Here4");
        }
        else if (rb.position.z >= 10 && rb.position.z < 30)
        {
            audioData.panStereo = 0.5f;
            Debug.Log("Here5");
        }
        else if (rb.position.z >= 30)
        {
            audioData.panStereo = 1;
            Debug.Log("Here6");
        }
    }
    private void FixedUpdate()
    {
        audioData.pitch = 0.35f;
        if (!audioData.isPlaying)
        {
            audioData.Play();
        }
        DynamicAudioChanges();
        //Vector3 oldVel = rb.velocity;
        //rb.velocity = Vector3.ClampMagnitude(oldVel, 40);
    }

    

}