using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public class Dbutil : MonoBehaviour
{
    public MySqlConnection conn = null;
    public MySqlCommand mySqlCommand = null;
    public MySqlDataReader mySqlDataReader = null;
    public string connectStr = "server=127.0.0.1;port=3306;";
    public string connectStr2 = "user=root;password=root;";



    //private void Start()
    //{
    //    SetConnectDB("test");
    //    GetConn();
    //    //string sql = "select * from master where wordId<20";
    //    //string sql = "select * from user where userId=@userId and name=@name";
    //    //string sql = "insert into user (name,pwd) values(@name,@pwd)";
    //    //string sql = "select * from user where name=90";
    //    string sql = "select * from user";
    //    sql = "update user set score=@score where userId=@userId";
    //    //Query(sql);
    //    //Query("select * from master where wordId<10");
    //    //AddDelete("insert into master(explaination,spell,wordLength)values('a','" + DateTime.Now + "',1)");
    //    //Query("select * from master where explaination='a'");
    //    //UpdateMysql("delete from master where explaination='a'");
    //    //UpdateMysql("update master set spell='aba' where wordId=1 and spell='abaa'");//字符串组合拼接
    //    //UpdateMysql("update master set spell='aaa' where wordId=@wordId and spell=@spell");//字符串组合拼接
    //    //Query("select * from master where explaination = 'a'");
    //    //mySqlDataReader.Close();
    //    //Debug.Log(IsUser(sql));
    //    //Debug.Log(Regist(sql, 1, "1"));
    //    //QuerySingle("select count(wordId) from master");
    //    //QuerySingle(sql);
    //    //GetAllUser(sql);
    //    UpdateUserScore(sql, 1, 1);
    //    Close();
    //}

    //private void Start()
    //{
    //    SetConnectDB("test");
    //    GetConn();
    //    string sql = "update master set isFamiliar=@isFamiliar where wordId=@wordId";
    //    UpdateWord(sql, 5, 20);
    //    Close();
    //}

    //private void Start()
    //{
    //    Save(1, "spell", "ex", "note", 1);
    //}
    //可以自定义数据库
    public void SetConnectDB(string dbname)
    {
        connectStr = connectStr +"database="+ dbname+";"+connectStr2;
    }
    //根据单词表 master medium simple，以及 单词ID 取得具体单词
    public Word GetWordById(int wordId,string tableName)
    {
        SetConnectDB("test");
        GetConn();
        string sql = "select * from "+tableName+" where wordId=" + wordId;
        Query(sql);
        mySqlDataReader.Read();
        Word w = new Word();
        w.WordId = mySqlDataReader.GetInt32("wordId");
        w.Spell = mySqlDataReader.GetString("spell");
        w.Explaination = mySqlDataReader.GetString("explaination");
        w.SentenceEN = mySqlDataReader.GetString("sentenceEN");
        w.SentenceCH = mySqlDataReader.GetString("sentenceCH");
        w.PronouncationURL = mySqlDataReader.GetString("pronouncationURL");
        w.LearnedTimes = mySqlDataReader.GetInt32("learnedTimes");
        w.IsFamiliar = mySqlDataReader.GetInt32("isFamiliar");
        return w;
    }
    //交朋友，向朋友表中插入数据
    public int AddFriend(int userId,int userId2)
    {
        SetConnectDB("test");
        GetConn();
        string sql = "insert into user_friend (userId,userId2) values(@userId,@userId2)";
        mySqlCommand = new MySqlCommand(sql,conn);
        mySqlCommand.Parameters.AddWithValue("userId", userId);
        mySqlCommand.Parameters.AddWithValue("userId2", userId2);
        int count= mySqlCommand.ExecuteNonQuery();
        return count;
    }
    //保存玩家游戏数据
    public int Save(int wordId,string spell,string explaination,string note,int userId)
    {
        SetConnectDB("test");
        GetConn();
        string sql = "insert into note(wordId,spell,explaination,note,userId) values(@wordId,@spell,@explaination,@note,@userId)";
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@wordId", wordId);
        mySqlCommand.Parameters.AddWithValue("@spell", spell);
        mySqlCommand.Parameters.AddWithValue("@explaination", explaination);
        mySqlCommand.Parameters.AddWithValue("@note", note);
        mySqlCommand.Parameters.AddWithValue("@userId", userId);
        int count = mySqlCommand.ExecuteNonQuery();
        return count;
    }
    //得到当前用户所有朋友的数据
    public List<User> GetFriendList(int currentUserId)
    {
        SetConnectDB("test");
        GetConn();
        string sql = "select * from user where userId=" + currentUserId;
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlDataReader = mySqlCommand.ExecuteReader();
        List<User> userList = new List<User>();
        User user = new User();
        while (mySqlDataReader.Read())
        {
            user.UserId = mySqlDataReader.GetInt32("userId");
            user.Name = mySqlDataReader.GetString("name");
            user.Pwd = mySqlDataReader.GetString("pwd");
            user.Score = mySqlDataReader.GetInt32("score");
            user.Date = mySqlDataReader.GetString("date");
            user.Email = mySqlDataReader.GetString("email");
            user.Phone = mySqlDataReader.GetString("phone");
            userList.Add(user);
        }
        return userList;
    }
    //根据 id 判断是不是朋友
    public bool IsFriend(string sql,int userId2)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@userId", userId2);
        mySqlDataReader = mySqlCommand.ExecuteReader();
        while (mySqlDataReader.Read())
        {
            if (mySqlDataReader.GetInt32("userId2") == userId2)
            {
                return true;
            }
        }
        return false;
    }

    //注册  用户数据
    public int Regist(string sql, string name,string pwd,string email,string phone)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@pwd", pwd);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlCommand.Parameters.AddWithValue("@email", email);
        mySqlCommand.Parameters.AddWithValue("@phone", phone);
        int count = mySqlCommand.ExecuteNonQuery();
        return count;
    }

    //更新单词的熟悉程度
    public void UpdateWord(string sql, int wordId,int isFamiliar)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@wordId", wordId);
        mySqlCommand.Parameters.AddWithValue("@isFamiliar", isFamiliar);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("更新 vocab 数据库数据isFamiliar= " + isFamiliar);
    }

    //更新玩家的得分数据
    public void UpdateUserScore(string sql, int score, int userId)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        Debug.Log(sql);
        mySqlCommand.Parameters.AddWithValue("@score", score);
        mySqlCommand.Parameters.AddWithValue("@userId", userId);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("数据库中受影响的数据行数" + count);
    }

    //根据昵称和密码来得到玩家的数据
    public User GetUser(string sql,string name,string pwd)
    {
        SetConnectDB("test");
        GetConn();
        User user = new User();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlCommand.Parameters.AddWithValue("@pwd", pwd);
        mySqlDataReader = mySqlCommand.ExecuteReader();
        while (mySqlDataReader.Read())
        {
            user.UserId = mySqlDataReader.GetInt32("userId");
            user.Name = mySqlDataReader.GetString("name");
            user.Pwd = mySqlDataReader.GetString("pwd");
            user.Score = mySqlDataReader.GetInt32("score");
            user.Date = mySqlDataReader.GetString("date");
            user.Email = mySqlDataReader.GetString("email");
            user.Phone = mySqlDataReader.GetString("phone");
        }
        return user;
    }

    public bool IsUser(string sql,string name,string pwd)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlCommand.Parameters.AddWithValue("@pwd", pwd);
        mySqlDataReader = mySqlCommand.ExecuteReader();

        bool flag = mySqlDataReader.Read();
        return flag;
    }

    internal bool CheckUserName(string sql, string name)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlDataReader = mySqlCommand.ExecuteReader();

        bool flag = mySqlDataReader.Read();
        return flag;
    }

    //与函数结合起来
    public void UpdateMysql(string sql,int score)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@score", score);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("数据库中受影响的数据行数" + count);
    }

    public void Query(string sql)
    {
        Debug.Log("经过组合之后的连接语句" + sql);

        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        //mySqlCommand.ExecuteReader();//query
        //mySqlCommand.ExecuteNonQuery();//insert update delete
        //mySqlCommand.ExecuteScalar();//return single value
        //mySqlCommand.Parameters.AddWithValue("@tName", tableName);
        mySqlDataReader = mySqlCommand.ExecuteReader();
        //while (mySqlDataReader.Read())
        //{
        //    Debug.Log(mySqlDataReader[0].ToString());
        //    Debug.Log(mySqlDataReader.GetInt32(0).ToString());
        //    Debug.Log(mySqlDataReader.GetInt32("wordId").ToString() + " " + mySqlDataReader.GetString("spell"));
        //}
    }

    public void GetAllUser(string sql)
    {
        SetConnectDB("test");
        GetConn();
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlDataReader = mySqlCommand.ExecuteReader();
    }


    public void GetConn()
    {
        conn = new MySqlConnection(connectStr);
        try
        {
            conn.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void Close()
    {
        if (mySqlDataReader != null)
        {
            mySqlDataReader.Close();
        }
        if (conn != null)
        {
            conn.Close();
        }
    }
}
