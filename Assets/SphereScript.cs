using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//depending on force, we can use a set of random numbers (0-5) for weak, (10-20) for OK, (25-40) for excellent and overshooting the game for too hard
public class SphereScript : MonoBehaviour
{
    public float duration = 1; // in seconds
    private float startTime;
    private Vector3 beginPoint = new Vector3(5, 0, 0);
    private Vector3 finalPoint = new Vector3(5, 0, 0);
    private Vector3 farPoint = new Vector3(0, 0, 0);
    private int c;

    void Start()
    {
        startTime = Time.time;
        c = Random.Range(1, 15);
        if ((c > 0) & (c < 5))
        {
            beginPoint = new Vector3(5, 0, Random.Range(1, 5));
        }
        else if ((c > 6) & (c < 10))
        {
            beginPoint = new Vector3(5, 0, Random.Range(10, 20));
        }
        else
        {
            beginPoint = new Vector3(5, 0, Random.Range(30, 60));
        }
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







