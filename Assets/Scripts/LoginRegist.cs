using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRegist 
{

    Dbutil dbutil = new Dbutil();

    public void GetConn()
    {
        dbutil.SetConnectDB("vocab");
        dbutil.GetConn();
    }
    public void Close()
    {
        dbutil.Close();
    }
    public bool CheckUser(string pwd,string name)
    {
        string sql = "select * from user where pwd=@pwd and name=@name";
        return dbutil.IsUser(sql, int.Parse(pwd), name);
    }

    public int Regist(int pwd,string name)
    {
        if (CheckUser(pwd.ToString(), name))
        {
            return 0;
        }
        int result = 0;
        string sql = "insert into user(name,pwd) values(@name,@pwd)";
        if (pwd != 0 || name != null)
        {
            result = dbutil.Regist(sql, pwd, name);
        }
        return result;
    }

    public int GetId(string pwd, string name)
    {
        string sql = "select userId from user pwd=@pwd and name=@name";
        dbutil.IsUser(sql, int.Parse(pwd), name);
        return dbutil.mySqlDataReader.GetInt32("userId");
    }
}
