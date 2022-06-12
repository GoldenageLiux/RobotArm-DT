using System.IO.Ports;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Threading;

public class Serial : MonoBehaviour
{
	public float j1, j2, j3, j4, j5, j6;
	public string status;
	public string port = "COM8";
	public Thread askThread;
	// Token: 0x06000035 RID: 53 RVA: 0x00002ED8 File Offset: 0x000010D8
	private void Awake()
	{
		this.OpenConnection();

	}

	private void Start()
	{
		askThread = new Thread(askFunc);
		askThread.Start();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002EE0 File Offset: 0x000010E0
	private void FixedUpdate()
	{

	}

	private void OnDestroy()
	{
		askThread.Abort();
	}
	// Token: 0x06000037 RID: 55 RVA: 0x00002F54 File Offset: 0x00001154
	public void OpenConnection()
	{
		if (SerialPort.GetPortNames().Length != 0)
		{
			Serial.IMU_port = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
		}
		if (Serial.IMU_port == null)
		{
			Debug.Log("Port == null");
			Application.Quit();
			return;
		}
		if (Serial.IMU_port.IsOpen)
		{
			Serial.IMU_port.Close();
			Debug.Log("Closing port, because it was already open!");
			return;
		}
		Serial.IMU_port.Open();
		Serial.IMU_port.ReadTimeout = 1;
		Debug.Log("Port Opened!");
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002FD7 File Offset: 0x000011D7
	private void OnApplicationQuit()
	{
		//Serial.IMU_port.Close();
	}

	public void add()
	{
		Serial.IMU_port.WriteLine("M21 G90 G00 X15.00 Y0.00 Z0.00 A0.00 B0.00 C0.00 F2000.00");
	}

	public void send_msg(string msg)
	{
		Serial.IMU_port.WriteLine(msg);
		//print("done");
	}

	public void ask()
	{
		Serial.IMU_port.WriteLine("?");
		Thread.Sleep(500);
		string text = Serial.IMU_port.ReadExisting();
		print(text);
	}

	public void askFunc()
	{
		while (true)
		{
			Debug.Log("asking!");
			Thread.Sleep(100);
			Serial.IMU_port.WriteLine("?");
			string text = Serial.IMU_port.ReadExisting();
			string[] array = new string[2];
			if (text != null)
			{
				array = text.Split(new char[]
				{
				'\n'
				});
			}
			text = array[0];

			//<Idle,Angle(ABCDXYZ):0.000,0.000,0.000,0.000,0.000,0.000,0.000,Cartesian coordinate(XYZ RxRyRz):
			//198.670,0.000,230.720,0.000,0.000,0.000,Pump PWM:0,Valve PWM:0,Motion_MODE:0>
			var reg = new Regex(
				@"<([^,]*),Angle\(ABCDXYZ\):([-\.\d,]*),Cartesian coordinate\(XYZ RxRyRz\):([-.\d,]*),Pump PWM:(\d+),Valve PWM:(\d+),Motion_MODE:(\d)>");
			if (text.Contains("<") && text.Contains(">"))
			{
				Match match = reg.Match(text);
				GroupCollection groups = match.Groups;
				status = groups[1].Value;
				string[] joint = new string[7];
				joint = groups[2].Value.Split(new char[] { ',' });
				j1 = float.Parse(joint[4]);
				j2 = float.Parse(joint[5]);
				j3 = float.Parse(joint[6]);
				j4 = float.Parse(joint[0]);
				j5 = float.Parse(joint[1]);
				j6 = float.Parse(joint[2]);
			}
		}
	}

	// Token: 0x0400002E RID: 46
	private static SerialPort IMU_port;

	// Token: 0x0400002F RID: 47
	public static float[] _quat = new float[4];

	// Token: 0x04000030 RID: 48
	public static bool _isUpdated = false;

	// Token: 0x04000031 RID: 49
	//private static int cnt = 0;

}

