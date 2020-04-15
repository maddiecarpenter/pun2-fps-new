using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WordBonusList : MonoBehaviour
{
    // scroll view 中的content，所有的 toggle 数据都是他的子物体
    public GameObject parentObj;
    //words 表示游戏中得到的单词 bonus;
    List<Word> words;
    //单词列表面板，初始状态隐藏
    public GameObject wordBonusList;
    //用于控制单词面板
    private bool isActive = false;

    public void UpdateListWord()
    {
        //使用 bool 属性判断单词面板是否展开
        isActive = !isActive;
        wordBonusList.gameObject.SetActive(isActive);

        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(parentObj.transform.GetChild(i).gameObject);
        }

        words = WordGenerator.list;
        //遍历所有的单词，填充到生成的 toggle 预制体里面
        foreach (Word w in words)
        {
            GameObject childObj = (GameObject)Resources.Load("Toggle");//寻找父物体
            GameObject prefabInstance = Instantiate(childObj);//实例化预制体
            prefabInstance.transform.parent = parentObj.transform;//寻找父物体

            //设置toggle 的页面显示数据
            prefabInstance.GetComponentsInChildren<Text>()[0].text = w.WordId.ToString();
            prefabInstance.GetComponentsInChildren<Text>()[1].text = w.Spell + " " + w.Explaination;
            prefabInstance.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public void DeleteWord()
    {
        //利用父物体下所有子物体数目创建一个整数数组存储 id，不一定存完
        int[] arr = new int[parentObj.transform.childCount];
        int count = 0;

        //创建 Transform 数组用来存储子物体 transform
        Transform[] obj = new Transform[parentObj.transform.childCount];

        //遍历父物体下面的子物体
        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            obj[i]= parentObj.transform.GetChild(i);
            //检查子物体 toggle 的开关是否打开
            if (obj[i].GetComponent<Toggle>().isOn)
            {
                //这些被选择的 toggleid，存入 id 数组
                arr[count] = int.Parse(obj[i].Find("bonusWordId").GetComponent<Text>().text);
                count++;
            }
        }

        //提示：foreach(Word w in words)//存在 list.Remove 的时候不能使用 foreach，只能用 for循环
        //words 存储的是战斗的时候存储在 list 中的单词，删除时需要把 list 中的根据选择的单词 id 进行相应单词删除
        for(int i = 0; i < words.Count; i++)
        {
            for (int j = 0 ; j < count; j++)
            {
                if (words[i].WordId == arr[j])
                {
                    WordGenerator.list.Remove(words[i]);
                }
            }
        }

        //从页面上删除被选中的单词 toggle
        for (int i = obj.Length - 1; i >= 0; i--)
        {
            if (obj[i].GetComponent<Toggle>().isOn)
            {
                Destroy(obj[i].transform.gameObject);
            }
        }
    }
}
