using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
    private int wordId;
    private string spell;
    private string explaination;
    private string sentenceEN;
    private string sentenceCH;
    private string pronouncationURL;
    private int learnedTimes;
    private int isFamiliar;

    //无参构造器
    public Word()
    {

    }

    //有参构造器
    public Word(int _wordId,string _spell,string _explaination, string _sentenceEN,string _sentenceCH,string _pronouncationURL,int _learnedTimes,int _isFamiliar)
    {
        wordId = _wordId;
        spell = _spell;
        explaination = _explaination;
        sentenceCH = _sentenceCH;
        sentenceEN = _sentenceEN;
        pronouncationURL = _pronouncationURL;
        learnedTimes = _learnedTimes;
        isFamiliar = _isFamiliar;
    }

    public int WordId
    {
        get { return wordId; }
        set { wordId = value; }
    }

    public string Spell
    {
        get { return spell; }
        set { spell = value; }
    }

    public string Explaination
    {
        get { return explaination; }
        set { explaination = value; }
    }

    public string SentenceEN
    {
        get { return sentenceEN; }
        set { sentenceEN = value; }
    }

    public string SentenceCH
    {
        get { return sentenceCH; }
        set { sentenceCH = value; }
    }

    public string PronouncationURL
    {
        get { return pronouncationURL; }
        set { pronouncationURL = value; }
    }

    public int IsFamiliar
    {
        get { return isFamiliar; }
        set { isFamiliar = value; }
    }

    public int LearnedTimes
    {
        get { return learnedTimes; }
        set { learnedTimes = value; }
    }


    public virtual string ToString()
    {
        return "wordId " + wordId 
        + " spell " + spell 
        + " explaination " + explaination 
        + " sentenceCH "  + sentenceCH 
        + " sentenceEN " + sentenceEN 
        + " learnedTimes " + learnedTimes
        + " isFamiliar "+ isFamiliar;
    }

}
