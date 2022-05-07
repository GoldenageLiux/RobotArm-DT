using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class control_label : MonoBehaviour
{
    public GameObject L1, L2, L3, L4, L5, L6;
    public GameObject J1, J2, J3, J4, J5, J6, Lock;

    public GameObject mirobot;
    public GameObject canvas;

    //float j1,j2,j3,j4,j5,j6;
    public bool flag = false;

    //bool change = false;
    // Start is called before the first frame update
    void Start()
    {
        close();
    }

    // Update is called once per frame
    void Update()
    {
        if(flag)
        {
            mirobot.GetComponent<control_zcy>().set_target(L1, J1.GetComponent<Slider>().value);
            mirobot.GetComponent<control_zcy>().set_target(L2, J2.GetComponent<Slider>().value);
            mirobot.GetComponent<control_zcy>().set_target(L3, J3.GetComponent<Slider>().value);
            mirobot.GetComponent<control_zcy>().set_target(L4, J4.GetComponent<Slider>().value);
            mirobot.GetComponent<control_zcy>().set_target(L5, J5.GetComponent<Slider>().value);
            mirobot.GetComponent<control_zcy>().set_target(L6, J6.GetComponent<Slider>().value);
        }

        if(canvas.GetComponent<Serial>().status=="Idle")
        {
            Lock.SetActive(false);
        }
        if(canvas.GetComponent<Serial>().status=="Run")
        {
            Lock.SetActive(true);
        }
        
    }

    public void SetTrue()
    {
        refresh();
        if(flag)
        {
            this.gameObject.SetActive(false);
            flag = false;
        }
        else{
            this.gameObject.SetActive(true);
            flag =true;
        }
        
    }

    public void refresh()
    {
        refresh_data(L1, J1);
        refresh_data(L2, J2);
        refresh_data(L3, J3);
        refresh_data(L4, J4);
        refresh_data(L5, J5);
        refresh_data(L6, J6);
    }

    void refresh_data(GameObject l, GameObject j)
    {
        j.GetComponent<Slider>().value = l.GetComponent<ArticulationBody>().xDrive.target;
        print("done");
    }

    public void close()
    {
        this.gameObject.SetActive(false);
        flag = false;
    }


}
