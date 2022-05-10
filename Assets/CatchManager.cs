using UnityEngine;
using System.Threading;
using System;


public class CatchManager : MonoBehaviour
{
    public float x;
    public float y;
    private float z;
    private float r;
    private int n = 0;
    private Thread catchThread;

    public GameObject serial;
    public GameObject green_prefab;
    public GameObject parent;
    public GameObject arm;
    public Serial serial_script;
    public CallPython callpython;

    // Start is called before the first frame update
    void Start()
    {
        //机械臂初始XY坐标
        x = 240.0f;
        y = 130.0f;
        //初始高度&夹爪角度
        z = 130.0f;
        r = 70.0f;

        serial_script = serial.GetComponent<Serial>();
        callpython = this.GetComponent<CallPython>();

        string msg = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
        serial.GetComponent<Serial>().send_msg(msg);

        catchThread = new Thread(catchThreadFunc);
        catchThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(arm.GetComponent<control_zcy>().set_all_target());
    }

    private void OnDestroy()
    {
        catchThread.Abort();
    }

    bool GetInfo(int n)
    {
        callpython.CallPythonAddHW(n);
        if (callpython.str != null)
        {
            if (callpython.str.Length == 2)
            { 
                UnityEngine.Debug.Log(n);
                return true;
            }
            return false;
        }
        return false;
    }

    float[] get_destination(int n)
    {
        //blue destination
        if(n==0)
        {
            return new float[2]{ 80.0f, -170.0f };
        }
        //green destination
        else if(n==1)
        {
            return new float[2] { 240.0f, -170.0f };
        }
        //red destination
        else
        {
            return new float[2] { 220.0f, 170.0f };
        }
    }

    void catchThreadFunc()
    {
        while(true)
        {
            serial_script.send_msg("M3S40");
            if (GetInfo(n))
            {
                UnityEngine.Debug.Log(String.Format("Color: {0} Has Be Captured!!!\n0 is blue, 1 is green, 2 is red", n));
                    
                //获得色块位置
                x = Convert.ToSingle(callpython.str[0]);
                y = Convert.ToSingle(callpython.str[1]);
                callpython.str = null;

                //GameObject cube = Instantiate(green_prefab) as GameObject;
                //cube.transform.position = new Vector3(x / 1000.0f, 0, y / 1000.0f);

                //机械臂移动至色块位置
                string msg_1 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);

                serial_script.send_msg(msg_1);
                Thread.Sleep(1000);

                //夹爪向下准备抓取
                z = 75.0f;
                string msg_2 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_2);
                Thread.Sleep(1000);

                //夹爪抓取物块
                string msg_3 = "M3S50";
                serial_script.send_msg(msg_3);
                Thread.Sleep(1000);

                //夹爪上升高度至130
                z = 130.0f;
                string msg_4 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_4);
                Thread.Sleep(1000);

                //送至对应色块目的位置
                x = get_destination(n)[0];
                y = get_destination(n)[1];
                UnityEngine.Debug.Log(String.Format("n = {0}", n));
                string msg_5 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_5);
                Thread.Sleep(2000);

                //松开夹爪
                string msg_6 = "M3S40";
                serial_script.send_msg(msg_6);
                //cube.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                UnityEngine.Debug.Log(String.Format("No Color: {0} Be Captured!!!\n0 is blue, 1 is green, 2 is red", n));
            }
            Thread.Sleep(2000);
            n++;
            n = n % 3;
        }
    }
}
