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

    private int stash;//current ammo
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
            //Debug.Log("no bullets in current stash,reload your gun");
            return false; 
        }
    }

    public void Reload()
    {
        //Debug.Log("before stash is " + stash);
        stash += clip;
        clip = Mathf.Min(clipSize, stash);
        stash -= clip;
        //Debug.Log("reload!!");
        //Debug.Log("current stash is " + stash);
        //Debug.Log("clip is " + clip);
    }

    public int GetStash() { return stash; }
    public void SetStash() { stash=ammo; }

    public int GetClip() { return clip; }
    public void SetClip() { clip=clipSize; }
}
