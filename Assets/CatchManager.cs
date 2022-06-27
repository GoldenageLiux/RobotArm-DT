using UnityEngine;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;

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
    public GameObject blue_prefab;
    public GameObject red_prefab;
    public GameObject parent;
    public GameObject arm;
    public Serial serial_script;
    private bool create_f, catch_f, release_f;
    private GameObject now_cube;
    public List<GameObject> m_cube;
    public List<int> color;
    public List<float> posx_que;
    public List<float> posy_que;
    public int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        m_cube = this.GetComponent<DrawAllCube>().cube_que;
        color = this.GetComponent<DrawAllCube>().color_que;
        posx_que = this.GetComponent<DrawAllCube>().posx_que;
        posy_que = this.GetComponent<DrawAllCube>().posy_que;
        create_f = false;
        catch_f = false;
        release_f = false;
        //机械臂初始XY坐标
        x = 240.0f;
        y = 130.0f;
        //初始高度&夹爪角度
        z = 130.0f;
        r = 75.0f;

        serial_script = serial.GetComponent<Serial>();
        string msg = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
        serial_script.send_msg(msg);

        catchThread = new Thread(catchThreadFunc);
        catchThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(arm.GetComponent<control_zcy>().set_all_target());
        if (create_f)
        {

        }
        if (catch_f)
        {
            now_cube.transform.position= new Vector3(parent.transform.position.x, parent.transform.position.y-0.03f, parent.transform.position.z);
            now_cube.transform.parent = parent.transform;
            
            catch_f = false;
        }
        if(release_f)
        {
            now_cube.GetComponent<Rigidbody>().useGravity = true;
            now_cube.transform.parent = serial.transform;
            release_f = false;             
        }
    }

    private void OnDestroy()
    {
        catchThread.Abort();
    }

    bool GetInfo()
    {
        
        if(index < m_cube.Count)
        {
            now_cube = (GameObject)m_cube[index++];
            return true;
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
        while (true)
        {
            serial_script.send_msg("M3S40");
            if (GetInfo())
            {

                UnityEngine.Debug.Log(String.Format("Color: {0} Has Be Captured!!!\n0 is blue, 1 is green, 2 is red", n));
                
                //获得色块位置
                x = posx_que[index-1];
                y = posy_que[index-1];

                create_f = true;
                Thread.Sleep(3000);
                //z = 200.0f;
                //机械臂移动至色块位置
                string msg_1 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, 130.0f, r);

                serial_script.send_msg(msg_1);
                Thread.Sleep(3000);

                //夹爪向下准备抓取
                //z = 75.0f;
                string msg_2 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, 75.0f, r);
                serial_script.send_msg(msg_2);

                Thread.Sleep(1000);

                //夹爪抓取物块
                string msg_3 = "M3S50";
                serial_script.send_msg(msg_3);

                Thread.Sleep(1000);
                catch_f = true;
                //夹爪上升高度至130
                z = 130.0f;
                string msg_4 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_4);
                Thread.Sleep(3000);

                n = color[index-1];
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
                Thread.Sleep(1000);
                release_f = true;

            }
            else
            {
                UnityEngine.Debug.Log(String.Format("No Color: {0} Be Captured!!!\n0 is blue, 1 is green, 2 is red", n));
            }
            Thread.Sleep(2000);
            //n++;
            //n = n % 3;
        }
    }
}
