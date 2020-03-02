using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class EnglishManager : MonoBehaviourPun
{
    public Transform escMenu;
    private bool isActive;
    public WordGenerator wordGenerator;
    public Word[] words;
    public Text spell;
    public Text explaination;
    int wordId;
    int isFamiliar;
    string sentenceCH;
    string sentenceEN;
    int currentIndex;
    int score=0;
    int userId;
    string sql;
    GameObject btnParent;

    Dbutil dbutil;

    private void Start()
    {
        escMenu.gameObject.SetActive(isActive);
        words = wordGenerator.GetWordsList();
       
        spell.text = words[0].Spell;
        explaination.text = words[0].Explaination;
        wordId = words[0].WordId;
        isFamiliar = words[0].IsFamiliar;
        sentenceCH = words[0].SentenceCH;
        sentenceEN = words[0].SentenceEN;


        currentIndex = 0;

        btnParent = GameObject.Find("Design/OptionBtn");

        foreach (Button btn in btnParent.GetComponentsInChildren<Button>())
        {
            //btn.onClick.AddListener(delegate () {
            //    switch (btn.GetComponentInChildren<Text>().text)
            //    {
            //        case "Easy":
            //            Debug.Log("easy........");
            //            break;
            //        case "Medium":
            //            break;
            //        case "Hard":
            //            break;
            //        default:
            //            break;
            //    }
            //});
            btn.onClick.AddListener(delegate ()
            {
                this.onClick(btn.name);
                Debug.Log("click btn once");
            });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            escMenu.gameObject.SetActive(isActive);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ExportTxt()
    {
        Debug.Log("to text");
    }

    public void ExportExcel()
    {
        Debug.Log("to excel");
    }

    void onClick(string text)
     {
        switch (text)
        {
            case "Easy":
                isFamiliar += 1;
                break;
            case "Medium":
                break;
            case "Hard":
                break;
            default:
                break;
        }

        Debug.Log("connect to db");
        dbutil = new Dbutil();
        dbutil.SetConnectDB("vocab");
        dbutil.GetConn();
        sql = "update master set isFamiliar=@isFamiliar where wordId=@wordId";
        dbutil.UpdateWord(sql, wordId, isFamiliar);
        dbutil.Close();
        Debug.Log("update isfamiliar");

        currentIndex++;
        Debug.Log("length of words " + words.Length);
        if (currentIndex >= words.Length)
        {
            Debug.Log("no next word");
            dbutil = new Dbutil();
            dbutil.SetConnectDB("vocab");
            dbutil.GetConn();
            userId = 1;score = 10;
            Debug.Log("finished courses");
            //将显示得分，并且把得分加入用户的数据库中
            sql = "update user set score=@score where userId=@userId";
            dbutil.UpdateUserScore(sql, score, userId);
            Debug.Log("save score,back to game");
            dbutil.Close();
            return ;
        }
        Debug.Log("current index: " + currentIndex);
        spell.text = words[currentIndex].Spell;
        explaination.text = words[currentIndex].Explaination;
        wordId = words[currentIndex].WordId;
        isFamiliar = words[currentIndex].IsFamiliar;
    }
}

