using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FriendList : MonoBehaviour
{
    //好友列表页面
    public GameObject friendListObj;
    //控制好友页面的显现
    private bool isActive=false;
    //Toggle 数据的父物体
    public GameObject parentObj;
    //存储所有用户
    List<User> users;
    LoginRegist loginRegist;

    public void UpdateFriendList()
    {
        isActive = !isActive;
        //显示好友列表
        friendListObj.SetActive(isActive);

        //每一次更新前都要把父物体的所有子物体销毁
        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(parentObj.transform.GetChild(i).gameObject);
        }
        loginRegist = new LoginRegist();
        loginRegist.GetUserList();
        users = loginRegist.allUsers;

        foreach (User user in users)
        {
            if (user.UserId != Launcher.currentUser.UserId)//排除了玩家本人
            {
                //排除了本人之后，把所有的玩家用户预制体 toggle 加载
                GameObject childObj = (GameObject)Resources.Load("PlayerToggle");
                GameObject prefabInstance = Instantiate(childObj);//实例化
                prefabInstance.transform.parent = parentObj.transform;//设置父物体
                prefabInstance.GetComponentsInChildren<Text>()[0].text = user.UserId.ToString();
                prefabInstance.GetComponentsInChildren<Text>()[1].text = user.Name + " " + user.Email;

                //检查玩家是不是当前玩家的好友
                if (CheckFriend(user.UserId))
                {
                    //如果是好友就设置成红色
                    prefabInstance.GetComponentsInChildren<Text>()[0].color = Color.red;
                    prefabInstance.GetComponentsInChildren<Text>()[1].color = Color.red;
                    Debug.Log(user.Name + " is friend");
                }
                else
                {
                    prefabInstance.GetComponentsInChildren<Text>()[0].color = Color.green;
                    prefabInstance.GetComponentsInChildren<Text>()[1].color = Color.green;
                    Debug.Log(user.Name + " not friend");
                }
                //设置相对的scale
                prefabInstance.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void AddFriend()
    {
        //创建一个整数数组来存储ID
        int[] userIds = new int[parentObj.transform.childCount];
        int count = 0;
        //创建一个 transform 数据存储子物体数据 toggle
        Transform[] obj = new Transform[parentObj.transform.childCount];

        for (int i = 0; i < parentObj.transform.childCount ; i++)
        {
            obj[i] = parentObj.transform.GetChild(i);
            //Toggle 开关打开同时它的颜色为红
            if (obj[i].GetComponent<Toggle>().isOn&&obj[i].GetComponentsInChildren<Text>()[0].color!=Color.red)
            {
                userIds[count] = int.Parse(obj[i].Find("FriendId").GetComponent<Text>().text);
                count++;
            }
        }

        Dbutil dbutil = new Dbutil();
        //首先更新数据库数据的好友信息
        foreach(int userId2 in userIds)
        {
            //之所以要加上两条数据，是因为数据表 user_friend table 是一个一对多的关系表；
            // user 和 user2 之间互为朋友
            dbutil.AddFriend(Launcher.currentUser.UserId, userId2);
            dbutil.AddFriend(userId2, Launcher.currentUser.UserId);
        }
        //然后更新页面上的数据
        UpdateFriendList();
        friendListObj.SetActive(true);
    }

    //根据玩家ID 来检查是否是自己的朋友
    public bool CheckFriend(int userId2)
    {
        Dbutil dbutil = new Dbutil();
        string sql = "select * from user_friend where userId=" + Launcher.currentUser.UserId;
        return dbutil.IsFriend(sql, userId2);
    }
}
