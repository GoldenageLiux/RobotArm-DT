using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class control_zcy : MonoBehaviour
{
    public GameObject j1;
    public GameObject j2;
    public GameObject j3;
    public GameObject j4;
    public GameObject j5, j6;
    public GameObject serial_;

    public float flag=0;

    public GameObject mode ;

    public GameObject label ;
    //public Thread set_target_thread;

    Vector3 v1, v2, v3, v4, v5, v6;
    // Start is called before the first frame update
    private void Start()
    {
        //mode = GameObject.Find("mode");
        v1 = new Vector3(0, 0, 0);
        v2 = new Vector3(-90, 0, 90);
        v3 = new Vector3(0, 0, 0);
        v4 = new Vector3(0, 0, 90);
        v5 = new Vector3(-90, 0, 90);
        v6 = new Vector3(0, 0, 90);

        //set_target_thread = new Thread(set_all_target);
        //set_target_thread.Start();
    }

    private void OnDestroy()
    {
        //set_target_thread.Abort();
    }

    // Update is called once per frame
    void Update()
    {
        flag = mode.GetComponent<Slider>().value;
        if(flag == 0)
        {
            refresh1_3(j1);
            refresh2(j2);
            refresh1_3(j3);
            refresh4_6(j4);
            refresh5(j5);
            refresh4_6(j6);
            if(Input.GetAxis("Vertical")!=0 || label.GetComponent<control_label>().flag)
            {
                print("updown");
                set_status();
            }
            else
            {
                StartCoroutine(set_all_target());
                //Debug.Log("setting!");
            }
        }

        else{
            label.GetComponent<control_label>().close();
            refresh1_3(j1);
            refresh2(j2);
            refresh1_3(j3);
            refresh4_6(j4);
            refresh5(j5);
            refresh4_6(j6);
            if(Input.GetKeyDown("return"))
            {
                set_status_flag_1();
            }
        }

    }

    public IEnumerator set_all_target()
    {
        yield return new WaitForSeconds(0.3f);
        set_target(j1, serial_.GetComponent<Serial>().j1);
        set_target(j2, serial_.GetComponent<Serial>().j2);
        set_target(j3, serial_.GetComponent<Serial>().j3);
        set_target(j4, serial_.GetComponent<Serial>().j4);
        set_target(j5, serial_.GetComponent<Serial>().j5);
        set_target(j6, serial_.GetComponent<Serial>().j6);
    }
    public void set_status()
    {
        string msg = String.Format("M21 G90 G00 X{0} Y{1} Z{2} A{3} B{4} C{5} F2000",
            j1.GetComponent<ArticulationBody>().xDrive.target,
            j2.GetComponent<ArticulationBody>().xDrive.target,
            j3.GetComponent<ArticulationBody>().xDrive.target,
            j4.GetComponent<ArticulationBody>().xDrive.target,
            j5.GetComponent<ArticulationBody>().xDrive.target,
            j6.GetComponent<ArticulationBody>().xDrive.target );
        print(msg);
        Thread.Sleep(50);
        serial_.GetComponent<Serial>().send_msg(msg);
    }

    public void set_status_flag_1()
    {
        if(flag==1)
        {
            string msg = String.Format("M21 G90 G00 X{0} Y{1} Z{2} A{3} B{4} C{5} F2000",
            j1.GetComponent<ArticulationBody>().xDrive.target,
            j2.GetComponent<ArticulationBody>().xDrive.target,
            j3.GetComponent<ArticulationBody>().xDrive.target,
            j4.GetComponent<ArticulationBody>().xDrive.target,
            j5.GetComponent<ArticulationBody>().xDrive.target,
            j6.GetComponent<ArticulationBody>().xDrive.target);
        print(msg);
        //Thread.Sleep(100);
        serial_.GetComponent<Serial>().send_msg(msg);
        }
        else{
            return;
        }
    }

    void refresh1_3(GameObject j)
    {
        Vector3 v = v1;
        ArticulationDrive currentDrive = j.GetComponent<ArticulationBody>().xDrive;
        v.y = -currentDrive.target + v1.y;
        v.x = v1.x;
        v.z = v1.z;
        j.transform.localEulerAngles = v;
    }
    void refresh4_6(GameObject j)
    {
        Vector3 v = v4;
        ArticulationDrive currentDrive = j.GetComponent<ArticulationBody>().xDrive;
        v.x = currentDrive.target + v4.x;
        v.y = v4.y;
        v.z = v4.z;
        j.transform.localEulerAngles = v;
    }

    void refresh2(GameObject j)
    {
        Vector3 v = v2;
        ArticulationDrive currentDrive = j.GetComponent<ArticulationBody>().xDrive;
        //v = v + new Vector3(currentDrive.target, 0, 0);
        v.x = currentDrive.target + v2.x;
        v.y = v2.y;
        v.z = v2.z;
        j.transform.localEulerAngles = v;
    }
    void refresh5(GameObject j)
    {
        Vector3 v = v2;
        ArticulationDrive currentDrive = j.GetComponent<ArticulationBody>().xDrive;
        //v = v + new Vector3(currentDrive.target, 0, 0);
        v.x = -currentDrive.target + v2.x;
        v.y = v2.y;
        v.z = v2.z;
        j.transform.localEulerAngles = v;
    }

    public void set_target(GameObject j, float j_)
    {
        ArticulationDrive currentDrive = j.GetComponent<ArticulationBody>().xDrive;
        //v = v + new Vector3(currentDrive.target, 0, 0);
        currentDrive.target = j_;
        j.GetComponent<ArticulationBody>().xDrive = currentDrive;
        refresh1_3(j1);
        refresh2(j2);
        refresh1_3(j3);
        refresh4_6(j4);
        refresh5(j5);
        refresh4_6(j6);
    }
}
