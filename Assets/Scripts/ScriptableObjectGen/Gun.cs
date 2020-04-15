using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Gun",menuName="Gun")]
public class Gun :ScriptableObject
{
    public string name;
    public int damage;
    public float aimSpeed;

    public int ammo;
    public int clipSize;

    public AudioClip gunShot;
    public float shotPitch;

    private int stash;//all bullets
    private int clip;//current clip

    public GameObject prefab;
    public int reloadTime;

    public void Init()
    {
        stash = ammo;
        clip = clipSize; 
    }

    public bool FireBullet()
    {
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else 
        {
            return false; 
        }
    }
    //比如说 ammo=50,clipSize=10
    //stash=50,clip=10
    //第一次 reload 发生之前，stash=50,clip=0
    //stash=10;clip=10
    public void Reload()
    {
        stash += clip;
        clip = Mathf.Min(clipSize, stash);
        stash -= clip;
    }

    public int GetStash() { return stash; }
    public void SetStash() { stash=ammo; }

    public int GetClip() { return clip; }
    public void SetClip() { clip=clipSize; }
}
