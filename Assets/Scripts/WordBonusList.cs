using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WordBonusList : MonoBehaviour
{
    GameObject parentObj;//content
    Word[] words;
    //private void Start()
    //{
    //    GameObject prefabInstance = Instantiate(childObj);
    //    prefabInstance.transform.parent = parentObj;
    //    prefabInstance.GetComponentInChildren<Text>().text = "hello";
    //    ListWord();

    //}

    public void UpdateListWord()
    {
        //GameObject obj = (GameObject)Resources.Load("fbx/ying1/ying1");//加载预制体
        //obj = Instantiate(obj);//实例化预制体
        //GameObject parent = GameObject.Find("parent");//寻找父物体
        //obj.transform.parent = parent.transform;//指定父物体


        //Word www = new Word();
        //www.Spell = "www";
        //www.Explaination = "www";
        //WordGenerator.list.Add(www);
        parentObj = (GameObject)Resources.Load("Content");
        Instantiate(parentObj, Vector3.zero, Quaternion.identity);
        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(parentObj.transform.GetChild(i));
        }

        words = WordGenerator.list.ToArray();
        foreach (Word w in words)
        {
            GameObject childObj = (GameObject)Resources.Load("Toggle");
            GameObject prefabInstance = Instantiate(childObj);
            prefabInstance.transform.parent = parentObj.transform;
            prefabInstance.GetComponentsInChildren<Text>()[0].text = w.WordId.ToString();
            prefabInstance.GetComponentsInChildren<Text>()[1].text = w.Spell + " " + w.Explaination;
            prefabInstance.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void DeleteWord()
    {
        //Text[] str =parentObj.childCount;
        int[] arr = new int[parentObj.transform.childCount];
        int count = 0;
        Transform[] obj = new Transform[parentObj.transform.childCount];

        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            obj[i]= parentObj.transform.GetChild(i);
            if (obj[i].GetComponent<Toggle>().isOn)
            {
                arr[count] = int.Parse(obj[i].Find("bonusWordId").GetComponent<Text>().text);
                count++;
            }
        }
        foreach(Word w in words)
        {
            for (int i = 0 ; i < count; i++)
            {
                if (w.WordId == arr[i])
                {
                    WordGenerator.list.Remove(w);
                    
                    Debug.Log(w.WordId);
                }
            }
        }
        for (int i = obj.Length - 1; i >= 0; i--)
        {
            if (obj[i].GetComponent<Toggle>().isOn)
            {
                Destroy(obj[i].transform);
            }
        }
        Destroy(parentObj);
        UpdateListWord();
        //Debug.Log(str.Length);
        //Debug.Log(str[0].text);
        //Debug.Log(str[0]);
        //Debug.Log(str[1]);
        //Debug.Log(str[2]);
    }
}
