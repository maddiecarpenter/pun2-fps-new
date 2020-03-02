using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public class Dbutil : MonoBehaviour
{
    MySqlConnection conn = null;
    MySqlCommand mySqlCommand = null;
    public MySqlDataReader mySqlDataReader = null;
    public string connectStr = "server=127.0.0.1;port=3306;";
    public string connectStr2 = "user=root;password=root;";



    private void Start()
    {
        SetConnectDB("vocab");
        GetConn();
        //string sql = "select * from master where wordId<20";
        //string sql = "select * from user where userId=@userId and name=@name";
        string sql = "insert into user (name,pwd) values(name=@name,pwd=@pwd)";
        //Query(sql);
        //Query("select * from master where wordId<10");
        //AddDelete("insert into master(explaination,spell,wordLength)values('a','" + DateTime.Now + "',1)");
        //Query("select * from master where explaination='a'");
        //UpdateMysql("delete from master where explaination='a'");
        //UpdateMysql("update master set spell='aba' where wordId=1 and spell='abaa'");//字符串组合拼接
        //UpdateMysql("update master set spell='aaa' where wordId=@wordId and spell=@spell");//字符串组合拼接
        //Query("select * from master where explaination = 'a'");
        //mySqlDataReader.Close();
        //Debug.Log(IsUser(sql, 1, "name1"));
        Debug.Log(Regist(sql, 1, "1"));
        //QuerySingle("select count(wordId) from master");
        Close();
    }
    public void SetConnectDB(string dbname)
    {
        connectStr = connectStr +"database="+ dbname+";"+connectStr2;
    }

    public void UpdateWordFamiliar()
    {

    }

    public void DeleteWord()
    {

    }

    public void AddWord()
    {

    }

    public void SelectPara(string[] paras)
    {

    }

    public int Regist(string sql,int pwd, string name)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@pwd", pwd);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        int count = mySqlCommand.ExecuteNonQuery();
        return count;
    }

    public void AddDelete(string sql,string name,int score)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlCommand.Parameters.AddWithValue("@score", score);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("数据库中受影响的数据行数" + count);
    }

    public void UpdateWord(string sql, int wordId,int isFamiliar)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@wordId", wordId);
        mySqlCommand.Parameters.AddWithValue("@isFamiliar", isFamiliar);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("更新 vocab 数据库数据isFamiliar= " + isFamiliar);
    }

    public void UpdateUserScore(string sql, int score, int userId)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        Debug.Log(sql);
        mySqlCommand.Parameters.AddWithValue("@score", score);
        mySqlCommand.Parameters.AddWithValue("@userId", userId);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("数据库中受影响的数据行数" + count);
    }

    public bool IsUser(string sql,int pwd,string name)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        mySqlCommand.Parameters.AddWithValue("@pwd", pwd);
        mySqlCommand.Parameters.AddWithValue("@name", name);
        mySqlDataReader = mySqlCommand.ExecuteReader();

        //mySqlDataReader.Read();

        //Debug.Log(mySqlDataReader.GetString("name"));
        return mySqlDataReader.Read();
        //while (mySqlDataReader.Read())
        //{
        //Debug.Log(mySqlDataReader[0].ToString());
        //Debug.Log(mySqlDataReader.GetInt32(0).ToString());
        //Debug.Log(mySqlDataReader.GetInt32("wordId").ToString() + " " + mySqlDataReader.GetString("spell"));
        //}
    }

    //与函数结合起来
    public void UpdateMysql(string sql,int score)
    {
        mySqlCommand = new MySqlCommand(sql, conn);

        //mySqlCommand.Parameters.AddWithValue("@id", 1);
        mySqlCommand.Parameters.AddWithValue("@score", score);
        int count = mySqlCommand.ExecuteNonQuery();
        Debug.Log("数据库中受影响的数据行数" + count);
    }
    void QuerySingle(string sql)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        object o = mySqlCommand.ExecuteScalar();
        int avg = Convert.ToInt32(o.ToString());
        Debug.Log(avg);
       
    }
    public void Query(string sql)
    {
        mySqlCommand = new MySqlCommand(sql, conn);
        //mySqlCommand.ExecuteReader();//query
        //mySqlCommand.ExecuteNonQuery();//insert update delete
        //mySqlCommand.ExecuteScalar();//return single value
        mySqlDataReader = mySqlCommand.ExecuteReader();
        Debug.Log("execute query in mysql");
        //while (mySqlDataReader.Read())
        //{
            //Debug.Log(mySqlDataReader[0].ToString());
            //Debug.Log(mySqlDataReader.GetInt32(0).ToString());
            //Debug.Log(mySqlDataReader.GetInt32("wordId").ToString() + " " + mySqlDataReader.GetString("spell"));
        //}
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
        finally
        {
            Debug.Log("数据库连接的状态： " + conn.State);
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
