using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        //ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToXZ()
    {
        this.transform.position = new Vector3(0,3,0);
        this.transform.forward = ball.transform.position - this.transform.position;
    }
}
