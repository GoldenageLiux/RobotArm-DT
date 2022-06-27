 
 
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

/// <summary>
/// 简单的Unity 直接调用 Python 的方法
/// </summary>
public class CallPython : MonoBehaviour
{
    // Start is called before the first frame update
    private string basePath;

    void Start()
    {
        basePath = @"F:\Projects\unityprojects\RobotArm-DT\Assets\python\";
    }

    public void CallPythonDraw()
    {
        CallDrawBase(basePath + "getposition.py");
    }
 
    /// <summary>
    /// Unity 调用 Python
    /// </summary>
    /// <param name="pyScriptPath">python 脚本路径</param>
    /// <param name="argvs">python 函数参数</param>
    public void CallDrawBase(string pyScriptPath) {
        Process process = new Process();
 
        // ptython 的解释器位置 python.exe
        process.StartInfo.FileName = @"D:\Program Files\Python\Python38\python.exe";
 
        
        UnityEngine.Debug.Log(pyScriptPath);

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = pyScriptPath;     // 路径+参数
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = false;        // 不显示执行窗口
 
        // 开始执行，获取执行输出，添加结果输出委托
        process.Start();
        //process.BeginOutputReadLine();
        //process.OutputDataReceived += new DataReceivedEventHandler(GetData);
        process.BeginOutputReadLine();
        process.OutputDataReceived += new DataReceivedEventHandler(Out_RecvData);
        Console.ReadLine();
        process.WaitForExit();

    }

    static void Out_RecvData(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            UnityEngine.Debug.Log(e.Data);

        }
    }

}