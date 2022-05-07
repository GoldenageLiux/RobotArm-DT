using System.IO.Ports;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine.UI;

public class management : MonoBehaviour
{
    public GameObject ball;

    public GameObject serial;

    public GameObject slider;

    public GameObject rotation;

    float x, y, z, r;

    // Start is called before the first frame update
    void Start()
    {
        serial.GetComponent<Serial>().send_msg("M20");
        serial.GetComponent<Serial>().send_msg("?");
        serial.GetComponent<Serial>().send_msg("M20 G90 G00 X200.00 Y0.00 Z80.00 A0.00 B0.00 C0.00 F2000.00");
    }

    // Update is called once per frame
    void Update()
    {
            print(Input.GetMouseButtonDown(0));
            x = 200+ball.transform.position.x*10;
            y = ball.transform.position.y*10;
            z = slider.GetComponent<Slider>().value;
            r = rotation.GetComponent<Slider>().value;
        
            string msg = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000",x,y,z,r);
            print(msg);
            Thread.Sleep(50);
            serial.GetComponent<Serial>().send_msg(msg);
    }

    public void open()
    {
        serial.GetComponent<Serial>().send_msg("M3S40");
    }

    public void close()
    {
        serial.GetComponent<Serial>().send_msg("M3S49");
    }
}
