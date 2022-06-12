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
    public GameObject blue_prefab;
    public GameObject red_prefab;
    public GameObject parent;
    public GameObject arm;
    public Serial serial_script;
    public CallPython callpython;
    private bool create_f, catch_f, release_f;
    private GameObject now_cube;

    // Start is called before the first frame update
    void Start()
    {
        create_f = false;
        catch_f = false;
        release_f = false;
        //��е�۳�ʼXY����
        x = 240.0f;
        y = 130.0f;
        //��ʼ�߶�&��צ�Ƕ�
        z = 130.0f;
        r = 0.0f;

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
        if (create_f)
        {
            if(n==0)
            {
                GameObject cube = Instantiate(blue_prefab) as GameObject;
                now_cube = cube;
            }
            else if (n == 1)
            {
                GameObject cube = Instantiate(green_prefab) as GameObject;
                now_cube = cube;
            }
            else
            {
                GameObject cube = Instantiate(red_prefab) as GameObject;
                now_cube = cube;
            }
            now_cube.transform.position = new Vector3(x / 1000.0f, 0, y / 1000.0f);
            create_f = false;
        }
        if (catch_f)
        {
            //now_cube.transform.position = new Vector3(0, 0.06f, 0);
            //now_cube.transform.position = parent.transform.position;
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
                    
                //���ɫ��λ��
                x = Convert.ToSingle(callpython.str[0]);
                y = Convert.ToSingle(callpython.str[1]);
                callpython.str = null;
                create_f = true;



                //��е���ƶ���ɫ��λ��
                string msg_1 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);

                serial_script.send_msg(msg_1);
                Thread.Sleep(1000);

                //��צ����׼��ץȡ
                z = 75.0f;
                string msg_2 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_2);
                
                Thread.Sleep(1000);

                //��צץȡ���
                string msg_3 = "M3S50";
                serial_script.send_msg(msg_3);
                
                Thread.Sleep(1000);
                catch_f = true;
                //��צ�����߶���130
                z = 130.0f;
                string msg_4 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_4);
                Thread.Sleep(3000);

                //������Ӧɫ��Ŀ��λ��
                x = get_destination(n)[0];
                y = get_destination(n)[1];
                UnityEngine.Debug.Log(String.Format("n = {0}", n));
                string msg_5 = String.Format("M20 G90 G00 X{0:N2} Y{1:N2} Z{2:N2} A0.00 B0.00 C{3:N2} F2000", x, y, z, r);
                serial_script.send_msg(msg_5);
                Thread.Sleep(2000);

                //�ɿ���צ
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
            n++;
            n = n % 3;
        }
    }
}
