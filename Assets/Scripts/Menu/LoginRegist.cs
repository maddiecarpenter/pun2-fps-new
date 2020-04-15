using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginRegist:MonoBehaviour
{
    public static List<User> userList = new List<User>();//已经登陆的 user
    public List<User> allUsers = new List<User>();//所有的 user
    private Text txt;
    private Dbutil dbutil;

    //得到所有的用户
    public void GetUserList()
    {
        dbutil = new Dbutil();
        string sql = "select * from user";
        dbutil.GetAllUser(sql);
        while (dbutil.mySqlDataReader.Read())
        {
            User user = new User();
            user.UserId = dbutil.mySqlDataReader.GetInt32("userId");
            user.Name = dbutil.mySqlDataReader.GetString("name");
            user.Pwd = dbutil.mySqlDataReader.GetString("pwd");
            allUsers.Add(user);
        }
    }

    //关闭连接
    public void Close()
    {
        dbutil.Close();
    }

    //根据名称和密码得到具体用户
    public User GetUser(string name,string pwd)
    {
        dbutil = new Dbutil();
        string sql = "select * from user where name=@name and pwd=@pwd";
        return dbutil.GetUser(sql,name,pwd);
    } 

    //检查用户是否存在
    public bool CheckUser(string name,string pwd)
    {
        dbutil = new Dbutil();
        string sql = "select * from user where name=@name and pwd=@pwd";
        //string sql = "select * from user where name=1 and pwd=1";
        return dbutil.IsUser(sql,name,pwd);
    }

    //检查用户名是否已经占用
    public bool CheckUserName(string name)
    {
        dbutil = new Dbutil();
        string sql = "select * from user where name=@name";
        //string sql = "select * from user where name=1 and pwd=1";
        return dbutil.CheckUserName(sql,name);
    }

    //注册
    public int Regist(string name, string pwd,string email,string phone)
    {
        dbutil = new Dbutil();
        if (CheckUserName(name))
        {
            txt = GameObject.Find("Canvas/Alert").GetComponent<Text>();
            txt.text = "名字已经被占用";
            return 0;
        }
        dbutil.mySqlDataReader.Close();

        int result = 0;
        string sql = "insert into user(name,pwd,email,phone,date) values(@name,@pwd,@email,@phone,now())";
        if (pwd!=null || name != null)
        {
            result = dbutil.Regist(sql, name, pwd,email,phone);
        }
        return result;
    }

    //根据密码和昵称得到玩家的 ID
    public int GetId(string pwd, string name)
    {
        string sql = "select userId from user pwd=@pwd and name=@name";
        dbutil.IsUser(sql,name,pwd);
        return dbutil.mySqlDataReader.GetInt32("userId");
    }

    //检查输入的数据是否规范
    public string CheckFormat(string type,string name, string pwd,string email,string phone)
    {
        if (name.Equals("") || pwd.Equals(""))
        {
            return "输入的昵称和密码不能为空";
        }
        else if (name.Length < 3 || name.Length > 10)
        {
            return "输入的昵称长度应该在3-10之间";
        }
        else if (pwd.Length < 3 || name.Length > 10)
        {
            return "输入的密码长度应该在3-10之间";
        }
        else
        {
            //如果密码不是数字组成，flag 设置为 false；
            bool flag = true;
            foreach (char c in pwd)
            {
                if (c > '9' || c < '0')
                {
                    flag = false;
                }
            }
            //数字符合要求；flag 是 true
            if (flag)
            {
                //注册的时候要比登陆的时候要多检查他的邮箱和电话是否规范
                if(type=="regist")
                {
                    if (email != null )
                    {
                        /*
                        有且只有一个“@”
                        “@”不可以是第一个位
                        */
                        if (email.IndexOf("@") == -1)
                        {
                            return "注册邮箱没有@";
                        }else if (email.IndexOf("@") != email.LastIndexOf("@"))
                        {
                            return "注册邮箱有多个@";
                        }else if (email.StartsWith("@"))
                        {
                            return "@不能是第一个";
                        }
                    }

                    if(phone!=null && phone.Length != 11)
                    {
                        return "注册的号码必须是 11 位数字";
                    }
                }

                return "format is right";
            }
            else
            {
                return "密码是 0-9 组成的整数";
            }
        }
    }

}
