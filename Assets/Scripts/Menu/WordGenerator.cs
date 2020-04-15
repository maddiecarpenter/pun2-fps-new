using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class WordGenerator : MonoBehaviour {

    public static List<Word> list=new List<Word>();//游戏中搜集单词
    public static List<Word> words= new List<Word>();//这是所有单词数据
    public static string tableName= "master";//默认的单词表，可以在登陆时候改变

    private void Start()
    {
        GetFromMyql();
        //GetFromJson();
    }

    //从 MySQL test 数据库中某个表得到数据
    public void GetFromMyql()
    {
        Dbutil dbutil = new Dbutil();
        dbutil.SetConnectDB("test");
        dbutil.GetConn();
        //按照熟悉程度排序
        string sql = "select * from "+tableName+" where wordId<=500 order by learnedTimes desc";
        dbutil.Query(sql);
        for (int i = 0; dbutil.mySqlDataReader.Read(); i++)
        {
            Word w = new Word();
            w.WordId = dbutil.mySqlDataReader.GetInt32("wordId");
            w.Spell = dbutil.mySqlDataReader.GetString("spell");
            w.Explaination = dbutil.mySqlDataReader.GetString("explaination");
            w.SentenceEN = dbutil.mySqlDataReader.GetString("sentenceEN");
            w.SentenceCH = dbutil.mySqlDataReader.GetString("sentenceCH");
            w.PronouncationURL = dbutil.mySqlDataReader.GetString("pronouncationURL");
            w.LearnedTimes = dbutil.mySqlDataReader.GetInt32("learnedTimes");
            w.IsFamiliar = dbutil.mySqlDataReader.GetInt32("isFamiliar");
            //符合条件的数据全部填充到了 words 中
            words.Add(w);
        }
        dbutil.Close();
    }

    //也可以直接从 JSon 数据表得到
    public void GetFromJson()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/MASTER.json");
        JsonReader jsonReader = new JsonReader(reader);
        JsonData jd = JsonMapper.ToObject(jsonReader);

        Word w = new Word();
        for (int i = 0; i < jd[0].Count; i++)
        {
            w.WordId = int.Parse(jd[0][i]["wordId"].ToString());
            w.Spell = jd[0][i]["spell"].ToString();
            w.Explaination = jd[0][i]["explaination"].ToString();
            w.SentenceCH = jd[0][i]["sentenceCH"].ToString();
            w.SentenceEN = jd[0][i]["sentenceEN"].ToString();
            w.LearnedTimes = int.Parse(jd[0][i]["learnedTimes"].ToString());
            w.PronouncationURL = jd[0][i]["pronouncationURL"].ToString();
            w.IsFamiliar = int.Parse(jd[0][i]["isFamiliar"].ToString());
            words.Add(w);
        }
    }

    //得到 words 中随机某个单词
    public static Word GetSingleWord()
    {
        int randomIndex =Random.Range(0, words.Count);
        Word randomWord = words[randomIndex];
        return randomWord;
    }

    //根据 单词 ID 得到具体的单词
    public static Word GetWordById(int wordId)
    {
        Dbutil dbutil = new Dbutil();
        return dbutil.GetWordById(wordId,tableName);
    }
}
