using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class port : MonoBehaviour
{
    public Serial app;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void connect()
    {
        app.OpenConnection();
    }

    public void j1()
    {
        app.add();
    }
}
