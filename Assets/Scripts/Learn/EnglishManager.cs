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
    public Transform noteEdit;
    private bool isActive;
    //public WordGenerator wordGenerator;
    public List<Word> words;
    public Text alert;
    public Text txtUserId;
    public Text txtName;
    public Text txtScore;
    
    public Text spell;
    public Text explaination;
    int wordId;
    int isFamiliar;
    string sentenceCH;
    string sentenceEN;
    int currentIndex;
    string pronouncationURL;
    int score;
    int userId;
    string sql;
    GameObject btnParent;

    Dbutil dbutil;

    private void Start()
    {
        escMenu.gameObject.SetActive(isActive);
        txtUserId.text= Launcher.currentUser.UserId.ToString();
        txtName.text = Launcher.currentUser.Name;
        txtScore.text = Launcher.currentUser.Score.ToString();
        alert.text = Launcher.currentUser.Score.ToString();

        words = WordGenerator.list;
       
        spell.text = words[0].Spell;
        explaination.text = words[0].Explaination;
        wordId = words[0].WordId;
        isFamiliar = words[0].IsFamiliar;
        sentenceCH = words[0].SentenceCH;
        sentenceEN = words[0].SentenceEN;
        pronouncationURL = words[0].PronouncationURL;
        score = Launcher.currentUser.Score; 

        //string urlMusic = "https://dictionary.blob.core.chinacloudapi.cn/media/audio/tom/ff/3c/FF3CF25481BE36DB7AB71852E96E9937.mp3";
        //string urlMusic = pronouncationURL;

        //StartCoroutine(SendGet(urlMusic));
       
        currentIndex = 0;

        btnParent = GameObject.Find("Design/OptionBtn");

        foreach (Button btn in btnParent.GetComponentsInChildren<Button>())
        {
            btn.onClick.AddListener(delegate ()
            {
                this.onClick(btn.name);
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
    public void Note()
    {
        alert.text = "editing note";
        if (noteEdit.gameObject.activeInHierarchy)
        {
            noteEdit.gameObject.SetActive(false);
        }
        else
        {
            noteEdit.gameObject.SetActive(true);
        }
    }

    public void Save()
    {
        alert.text = "save";
        Text note= GameObject.Find("Design/NoteEditing/InputField/Text").GetComponent<Text>();
        //int count= dbutil.Save(wordId.ToString(), spell.text, explaination.text,note.text, "1");
        dbutil = new Dbutil();
        int count = dbutil.Save(1, "spell", "ex", "note", 1);
        if (count == 1)
        {
            alert.text = "笔记添加成功";
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ExportTxt()
    {
        alert.text = "exporting to txt";
        ExportImport.SaveTxt(words);
    }

    public void ImportTxt()
    {
        alert.text = "import txt";
        words = ExportImport.LoadTxt();
        spell.text = words[0].Spell;
        explaination.text = words[0].Explaination;
        Debug.Log(words[0].Spell);
        currentIndex = 0;
    }

    void onClick(string text)
     {
        switch (text)
        {
            case "Easy":
                isFamiliar += 2;
                score+=2;
                txtScore.text = score.ToString();
                Debug.Log("挺简单的");
                break;
            case "Medium":
                isFamiliar += 1;
                score += 1;
                txtScore.text = score.ToString();
                Debug.Log("中等难度");

                break;
            case "Hard":
                //isFamiliar += -2;
                //score += -2;
                //txtScore.text = score.ToString();
                Debug.Log("挺困难的");
                break;
            default:
                break;
        }

        dbutil = new Dbutil();
        sql = "update master set isFamiliar=@isFamiliar where wordId=@wordId";
        dbutil.UpdateWord(sql, wordId, isFamiliar);
        dbutil.Close();

        currentIndex++;
        if (currentIndex >= words.Count)
        {
            dbutil = new Dbutil();
            userId = Launcher.currentUser.UserId;
            alert.text = "finish course ,no word left";
            //将显示得分，并且把得分加入用户的数据库中
            sql = "update user set score=@score where userId=@userId";
            dbutil.UpdateUserScore(sql,score,userId);
            dbutil.Close();
            currentIndex = 1;
            return ;
        }
        spell.text = words[currentIndex].Spell;
        explaination.text = words[currentIndex].Explaination;
        wordId = words[currentIndex].WordId;
        isFamiliar = words[currentIndex].IsFamiliar;
        pronouncationURL = words[currentIndex].PronouncationURL;
        //Debug.Log(pronouncationURL);

        //string urlMusic = "https://dictionary.blob.core.chinacloudapi.cn/media/audio/tom/ff/3c/FF3CF25481BE36DB7AB71852E96E9937.mp3";
        //string urlMusic = pronouncationURL;
        //StartCoroutine(SendGet(urlMusic));
    }


    public IEnumerator SendGet(string url)
    {
        WWW www = new WWW(url);

        yield return www;

        if (string.IsNullOrEmpty(www.error))

        {
            //GetComponent<AudioSource>().material.SetTexture("_MainTex", www.texture);

            //GetComponent<AudioSource>().clip = www.GetAudioClip(true, true, AudioType.MPEG);
            GetComponent<AudioSource>().clip = NAudioPlayer.FromMp3Data(www.bytes);

            GetComponent<AudioSource>().Play();
        }
        //WWW www = new WWW(musicUrl);
        //while (!www.isDone)
        //{
        //    yield return 0;
        //}
        //GetComponent<AudioSource>().clip = NAudioPlayer.FromMp3Data(www.bytes);
    }
}

