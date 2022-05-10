 
 
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
    public string[] str;
    void Start()
    {
        basePath = @"F:\Projects\unityprojects\RobotArm-DT\Assets\python\";
    }
 
    public void CallPythonAddHW(int a)
    {
        CallPythonBase(basePath + "test.pyw", a.ToString());
    }
 
    /// <summary>
    /// Unity 调用 Python
    /// </summary>
    /// <param name="pyScriptPath">python 脚本路径</param>
    /// <param name="argvs">python 函数参数</param>
    public void CallPythonBase(string pyScriptPath, params string[] argvs) {
        Process process = new Process();
 
        // ptython 的解释器位置 python.exe
        process.StartInfo.FileName = @"D:\Program Files\Python\Python38\python.exe";
 
        // 判断是否有参数（也可不用添加这个判断）
        if (argvs != null)
        {
            // 添加参数 （组合成：python xxx/xxx/xxx/test.python param1 param2）
            foreach (string item in argvs)
            {
                pyScriptPath += " " + item;
            }
        }
        UnityEngine.Debug.Log(pyScriptPath);

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = pyScriptPath;     // 路径+参数
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;        // 不显示执行窗口
 
        // 开始执行，获取执行输出，添加结果输出委托
        process.Start();
        process.BeginOutputReadLine();
        process.OutputDataReceived += new DataReceivedEventHandler(GetData);
        process.WaitForExit();
        
        
    }
 
    /// <summary>
    /// 输出结果委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void GetData(object sender, DataReceivedEventArgs e){        
 
        // 结果不为空才打印（后期可以自己添加类型不同的处理委托）
        if (string.IsNullOrEmpty(e.Data)==false)
        {
            if(e.Data[0] != '[' && e.Data[0] != ' ')
            {
                //UnityEngine.Debug.Log(e.Data);
                str = e.Data.Split(',');
                UnityEngine.Debug.Log(str[0]);
                UnityEngine.Debug.Log(str[1]);
            }
            
        }
    }
}