using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {
    private static string[] wordList = new string[40];//for fps
    public static List<Word> list=new List<Word>();
    private static Word[] words = new Word[40];
    //{   
         //                           "sidewalk", "robin", "three", "protect", "periodic",
									//"somber", "majestic", "jump", "pretty", "wound", "jazzy",
									//"memory", "join", "crack", "grade", "boot", "cloudy", "sick",
									//"mug", "hot", "tart", "dangerous", "mother", "rustic", "economic",
									//"weird", "cut", "parallel", "wood", "encouraging", "interrupt",
									//"guide", "long", "chief", "mom", "signal", "rely", "abortive",
									//"hair", "representative", "earth", "grate", "proud", "feel",
									//"hilarious", "addition", "silent", "play", "floor", "numerous",
									//"friend", "pizzas", "building", "organic", "past", "mute", "unusual",
									//"mellow", "analyse", "crate", "homely", "protest", "painstaking",
									//"society", "head", "female", "eager", "heap", "dramatic", "present",
									//"sin", "box", "pies", "awesome", "root", "available", "sleet", "wax",
									//"boring", "smash", "anger", "tasty", "spare", "tray", "daffy", "scarce",
									//"account", "spot", "thought", "distinct", "nimble", "practise", "cream",
									//"ablaze", "thoughtless", "love", "verdict", "giant"    
                                    //};
    private void Start()
    {
        GetFromMyql();

    }

    public void GetFromMyql()
    {
        //string connectStr = "server=127.0.0.1;port=3306;database=vocab;user=root;password=root";

        Dbutil dbutil = new Dbutil();
        dbutil.SetConnectDB("vocab");
        dbutil.GetConn();
        string sql = "select * from master where wordId<=40 order by learnedTimes desc";
        dbutil.Query(sql);
        for (int i = 0; dbutil.mySqlDataReader.Read(); i++)
        {
            wordList[i] = dbutil.mySqlDataReader.GetString("spell");
            words[i] = new Word(
            dbutil.mySqlDataReader.GetInt32("wordId"),
            dbutil.mySqlDataReader.GetString("spell"),
            dbutil.mySqlDataReader.GetString("explaination"),
            dbutil.mySqlDataReader.GetString("sentenceEN"),
            dbutil.mySqlDataReader.GetString("sentenceCH"),
            dbutil.mySqlDataReader.GetString("pronouncationURL"),
            dbutil.mySqlDataReader.GetInt32("learnedTimes"),
            dbutil.mySqlDataReader.GetInt32("isFamiliar")
            );
            //list.Add(words[i]);
            
        }
        
        dbutil.Close();
        Debug.Log("close db");
    }

 //   public static string GetRandomWord ()
	//{
 //       int randomIndex = UnityEngine.Random.Range(0, wordList.Length);
	//	string randomWord = wordList[randomIndex];
	//	return randomWord;
	//}

    public Word[] GetWordsList()
    {
        return list.ToArray();
        //return words;
    }
    

    public static Word GetSingleWord()
    {
        int randomIndex = UnityEngine.Random.Range(0, words.Length);
        Word randomWord = words[randomIndex];
        return randomWord;
    }
}
