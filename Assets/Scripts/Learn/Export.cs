using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ExportImport : MonoBehaviour
{
    //保存单词数据
    public static void SaveTxt(List<Word> words)
    {
        try
        {
            string path = Application.dataPath + "/wordlist.txt";

            if (File.Exists(path))//判断文件是否存在
            {
                File.Delete(path);
            }
            FileStream file = File.Create(path);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, words);//单词对象列表进行序列化
            file.Close();
            Debug.Log("保存单词列表成功");
        }
        catch
        {
            Debug.Log("有错误出现");
        }
    }

    //加载单词数据
    public static List<Word> LoadTxt()
    {
        List<Word> ret = new List<Word>();
        //Word ret = new Word();
        try
        {
            string path = Application.dataPath + "/wordlist.txt";
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                ret = (List<Word>)bf.Deserialize(file);//单词对象列表进行反序列化
                Debug.Log("load Success");
            }
            else
            {
                Debug.Log("没有文件可以加在");
            }
        }
        catch
        {
            Debug.Log("加载失败");
        }
        return ret;
    }
}
