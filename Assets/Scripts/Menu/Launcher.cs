using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

//第一个游戏场景，登录注册场景
public class Launcher : MonoBehaviourPunCallbacks
{
    //登录注册管理
    private LoginRegist loginRegist;
    //页面填充的数据
    public static string name;
    public static string pwd;
    public static string email;
    public static string phone;
    //页面提示，使用红色字体显示
    private Text txt;
    //设置当前用户，以便于后续引用
    public static User currentUser;
    //注册页面
    public GameObject registMenu;
    //主页面
    public GameObject menu;
    //控制登录主页面，注册小页面显示与否
    private bool isActive = false;

    public Dropdown dropdown;
    //所有玩家进入同一个场景
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //Connect();
        //最开始只显示主页面，不显示注册页面
        registMenu.SetActive(isActive);
        //找到场景中的提示物体得到相应的组件
        txt= GameObject.Find("Canvas/Alert").GetComponent<Text>();

        dropdown.onValueChanged.AddListener((int v) => OnValueChange(v));
    }

    //连接 master
    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED TO MASTER!");
        Debug.Log("TRY TO JOIN ROOM");
        Join();
    }

    private void OnFailedToConnectToMasterServer()
    {
        Debug.Log("失败连接 master");
    }

    //加入房间
    public override void OnJoinedRoom()
    {
        Debug.Log("joined room success,load scene start game");
        StartGame();
    }

    //加入随机房间失败
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("join room failed, try to create room!");
        Create();
    }

    //加入房间
    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom("created a new room");
    }

    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    //点击 connect 的按钮
    public void Connect()
    {
        loginRegist = new LoginRegist();
        name = GameObject.Find("Menu/name/name").GetComponent<Text>().text;
        pwd = GameObject.Find("Menu/pwd/pwd").GetComponent<Text>().text;
        txt.text=loginRegist.CheckFormat("login",name, pwd,"","");
        txt.text = "format is right";
        if(txt.text=="format is right")//检查填入的数据是否符合规则
        {
            if (loginRegist.CheckUser(name, pwd))//检查玩家是否存在
            {
                currentUser = loginRegist.GetUser(name, pwd);
                if (LoginRegist.userList.Contains(currentUser))//检查玩家是否已经登入
                {
                    txt.text = "user already login in";
                }
                else
                {
                    Debug.Log("Trying to connect...");
                    PhotonNetwork.GameVersion = "0.0.0";
                    PhotonNetwork.ConnectUsingSettings();
                    LoginRegist.userList.Add(currentUser);
                }
            }
        }
    }

    //点击注册页面的注册按钮，可以注册进入 MySQL 数据库
    public void Regist()
    {
        loginRegist = new LoginRegist();
        name = GameObject.Find("RegistMenu/name/name").GetComponent<Text>().text;
        pwd = GameObject.Find("RegistMenu/pwd/pwd").GetComponent<Text>().text;
        email = GameObject.Find("RegistMenu/email/email").GetComponent<Text>().text;
        phone = GameObject.Find("RegistMenu/phone/phone").GetComponent<Text>().text;

        txt.text = loginRegist.CheckFormat("regist", name, pwd, email, phone);

        if (txt.text == "format is right")
        {
            if (loginRegist.Regist(name, pwd, email, phone) == 1)
            {
                txt.text = "regist success";
                txt.color = Color.red;
            }
            else
            {
                txt.text = "Alert: user already in game database";
                txt.color = Color.red;
            }
        }
    }

    //点击 regist按钮弹出注册页面
    public void RegistMenu()
    {
        //当注册小页面显示或者不显示
        registMenu.SetActive(!isActive);
        //登陆主页面和注册小页面相反
        menu.SetActive(isActive);
        isActive = !isActive;
    }

    //public void DropSelect()
    //{
    //    Dropdown.OptionData data = new Dropdown.OptionData();
    //    data.text = "111";
    //    dropdown.options.Add(data);

    //    dropdown.onValueChanged.AddListener((int v) => OnValueChange(v));
    //}

    private void OnValueChange(int v)
    {
        switch (v)
        {
            case 0:
                WordGenerator.tableName = "master";
                break;
            case 1:
                WordGenerator.tableName = "medium";
                break;
            case 2:
                WordGenerator.tableName = "simple";
                break;
            default:
                txt.text = "没有正确选择单词库";
                break;
        }
        Debug.Log("选择了第" + v + "个，单词表"+WordGenerator.tableName);
    }
}
