using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//巡游模式摄像机控制
public class CameraMove : MonoBehaviour
{
 
 public static CameraMove Instance = null;
 
 private Vector3 dirVector3;
 private Vector3 rotaVector3;
 private float paramater = 0.1f;
 //旋转参数
 private float xspeed = -0.05f;
 private float yspeed = 0.1f;
 
 private float dis;
 
 void Awake ()
 {
 Instance = this;
 }
 
 private void Start()
 {
 rotaVector3 = transform.localEulerAngles;
 //dis = UIFuc.Instance.Dis;
 }
 
 // Update is called once per frame
 void FixedUpdate ()
 {
 //旋转
 if (Input.GetMouseButton(1))
 {
  rotaVector3.y += Input.GetAxis("Horizontal") * yspeed;
  rotaVector3.x += Input.GetAxis("Vertical") * xspeed;
  transform.rotation = Quaternion.Euler(rotaVector3);
 }
 
 //移动
 dirVector3 =Vector3.zero;
 
 if (Input.GetKey(KeyCode.I))
 {
  if(Input.GetKey(KeyCode.LeftShift)) dirVector3.z = 3;
  else dirVector3.z = 1;
 }
 if (Input.GetKey(KeyCode.K))
 {
  if (Input.GetKey(KeyCode.LeftShift)) dirVector3.z = -3;
  else dirVector3.z = -1;
 }
 if (Input.GetKey(KeyCode.J))
 {
  if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = -3;
  else dirVector3.x = -1;
 }
 if (Input.GetKey(KeyCode.L))
 {
  if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = 3;
  else dirVector3.x = 1;
 }
 if (Input.GetKey(KeyCode.U))
 {
  if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = -3;
  else dirVector3.y = -1;
 }
 if (Input.GetKey(KeyCode.O))
 {
  if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = 3;
  else dirVector3.y = 1;
 }
 transform.Translate(dirVector3 * paramater,Space.Self);
 //限制摄像机范围
 //transform.position = Vector3.ClampMagnitude(transform.position, dis);
 }
}