using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
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

    void Start()

    {
        string urlMusic = "https://dictionary.blob.core.chinacloudapi.cn/media/audio/tom/ff/3c/FF3CF25481BE36DB7AB71852E96E9937.mp3";

        StartCoroutine(SendGet(urlMusic));
    }

}
