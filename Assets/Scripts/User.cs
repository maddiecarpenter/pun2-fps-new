using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class User 
{
    private int userId;
    private string name;
    private string pwd;
    private int score;

    public User()
    {

    }

    public User(int _userId,string _name,string _pwd,int _score)
    {
        userId = _userId;
        name = _name;
        pwd = _pwd;
        score = _score;
    }

    public int UserId
    {
        get { return userId; }
        set { userId = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Pwd
    {
        get { return pwd; }
        set { pwd = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

}
