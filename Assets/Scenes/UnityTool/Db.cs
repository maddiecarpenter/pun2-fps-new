using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
public class Db : MonoBehaviour
{
    string connStr = "server=mysql;host=localhost;database=test;port=3306;user=root;pwd=root";
    MySqlCommand command;
    MySqlConnection connection;


}
