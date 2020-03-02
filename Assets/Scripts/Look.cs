using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Look : MonoBehaviourPun
{
    public static bool cursorLocked=true;
    public Transform player;//transform whole player
    public Transform normalCam;//transform the camera
    public Transform weapon;

    public float xsensitivity = 1000;
    public float ysensitivity= 1000;
    public float maxAngle=60;
    private Quaternion camCenter;

    private void Start()
    {
        camCenter = normalCam.localRotation;
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        SetY();
        SetX();
        UpdateCursor();
    }

    void SetY()
    {
        float t_input = Input.GetAxis("Mouse Y") * ysensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = normalCam.localRotation * t_adj;//cannot add ,must multiplay

        if (Quaternion.Angle(camCenter, t_delta) < maxAngle)
        {
            normalCam.localRotation = t_delta;
        }
        weapon.localRotation = normalCam.localRotation;
    }

    void SetX()
    {
        float t_input = Input.GetAxis("Mouse X") * xsensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * t_adj;//cannot add ,must multiplay
        player.localRotation = t_delta;
    }

    void UpdateCursor()
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;
            }
        }

    }

}
