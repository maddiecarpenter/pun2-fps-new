using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Look : MonoBehaviourPun
{
    //玩家整个预制体
    public Transform player;
    //玩家的眼睛
    public Transform normalCam;
    //玩家武器
    public Transform weapon;
    //x  y轴方向的敏感度
    public float xsensitivity = 1000;
    public float ysensitivity= 1000;
    //向上抬头最大的角度
    public float maxAngle=60;
    //localrotation
    private Quaternion camStart;

    private void Start()
    {
        //记录相机初始位置
        camStart = normalCam.localRotation;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        SetY();
        SetX();
    }

    void SetY()
    {
        //检测鼠标或者触控盘往上
        float t_input = Input.GetAxis("Mouse Y") * ysensitivity * Time.deltaTime;
        //围绕着 vector3.left 旋转
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_end = normalCam.localRotation * t_adj;
        //因为向上抬头看天空，有一个限制的角度，判断刚开始的角度和之后的角度之间是否小于 60
        if (Quaternion.Angle(camStart,t_end) < maxAngle)
        {
            normalCam.localRotation = normalCam.localRotation * t_adj;
        }
        weapon.localRotation = normalCam.localRotation;
    }

    void SetX()
    {
        //检测鼠标或者触控在 方向的移动，乘以敏感度；利用 time.deltaTime 保证不同帧率的平台上面保证准确性
        float t_input = Input.GetAxis("Mouse X") * xsensitivity * Time.deltaTime;
        //玩家左右移动之时，其实围绕vector3.up 方向
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        //不能加 quaternion 只能使用乘法
        Quaternion t_end = player.localRotation * t_adj;
        player.localRotation = t_end;
    }
}
