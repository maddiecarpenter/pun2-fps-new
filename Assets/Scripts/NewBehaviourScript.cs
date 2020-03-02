using System;
using MySql.Data.MySqlClient;
using UnityEngine;

   public class NewBehaviourScript:MonoBehaviour
    {
         void Start() { 

            // 数据库配置
            string connStr = "Database=vocab;datasource=127.0.0.1;port=3306;user=root;pwd=root;";
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            #region 查询
            // 查询user表中所有条目
            MySqlCommand cmd = new MySqlCommand("select * from master", conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            // 逐行读取数据
            while (reader.Read())
            {
            //string username = reader.GetString("username");
            //string password = reader.GetString("password");
            //Console.WriteLine(username + ":" + password);
            Debug.Log(reader.GetInt32(0).ToString());
            //Debug.Log(reader.GetString(2));
        }

        reader.Close();
            #endregion

            #region 插入
//            // 正常插入一条数据
//            string username = "lj";string password = "6666";
//            MySqlCommand cmd = new MySqlCommand("insert into user set username ='" + username + "'" + ",password='" + password + "'", conn);
//            cmd.ExecuteNonQuery();
            
//            // sql 注入，会删除数据库
//            string username = "lj"; string password = "6666'; delete from user;";
//            MySqlCommand cmd = new MySqlCommand("insert into user set username ='" + username + "'" + ",password='" + password + "'", conn);
//            cmd.ExecuteNonQuery();

//            // 防止注入,不会执行删除数据库语句
//            string username = "lj"; string password = "6666'; delete from user;";
//            MySqlCommand cmd = new MySqlCommand("insert into user set username=@uid , password = @pwd", conn);
//            cmd.Parameters.AddWithValue("uid", username);
//            cmd.Parameters.AddWithValue("pwd", password);
//            cmd.ExecuteNonQuery();
            #endregion

            #region 删除
//            // 删除 id 为 6 的条目
//            MySqlCommand cmd = new MySqlCommand("delete from user where id = @id", conn);
//            cmd.Parameters.AddWithValue("id", 6);
//
//            cmd.ExecuteNonQuery();
            #endregion

            //#region 更新
            //// 将 id 为 7 的条目 pwd 修改为 lll
            //MySqlCommand cmd = new MySqlCommand("update user set password = @pwd where id = 7", conn);
            //cmd.Parameters.AddWithValue("pwd", "lll");

            //cmd.ExecuteNonQuery();
            //#endregion

            //conn.Close();

            //Console.ReadKey();
        }
    }
