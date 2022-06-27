 
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading;

public class DrawAllCube : MonoBehaviour
{
    public GameObject blue_prefab;
    public GameObject green_prefab;
    public GameObject red_prefab;
    public CallPython callpython;
    private List<Newtonsoft.Json.Linq.JToken> blue,green,red;
    private float x;
    private float y;
    
    public List<GameObject> cube_que;
    public List<int> color_que;
    public List<float> posx_que;
    public List<float> posy_que;

    private string fileName;
    // Start is called before the first frame update
    void Start()
    {
        callpython = this.GetComponent<CallPython>();
        callpython.CallPythonDraw();
        fileName = @"F:\Projects\unityprojects\RobotArm-DT\Assets\python\positions.json";
    }

    // Update is called once per frame
    void Update()
    {
        if(File.Exists(fileName)) 
        { 
            StreamReader file = File.OpenText(fileName);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);
            blue = jsonObject["blue"].ToList();
            green = jsonObject["green"].ToList();
            red = jsonObject["red"].ToList();
            file.Close();

            Draw(blue, blue_prefab);
            color_que.Add(0);
            color_que.Add(0);
            Draw(green, green_prefab);
            color_que.Add(1);
            color_que.Add(1);
            Draw(red, red_prefab);
            color_que.Add(2);
            color_que.Add(2);
        }
        else
        {
            
        }
    }

    void Draw(List<Newtonsoft.Json.Linq.JToken> colorPosition, GameObject prefab)
    {
        foreach(Newtonsoft.Json.Linq.JToken position in colorPosition)
        {
            x = (float)position[0];
            y = (float)position[1];
            posx_que.Add(x);
            posy_que.Add(y);
            GameObject cube = Instantiate(prefab) as GameObject;
            cube.transform.position = new Vector3(x / 1000.0f, 0, y / 1000.0f);
            cube_que.Add(cube);
            
        }
    }

    void OnDestroy()
    {

    }

}
