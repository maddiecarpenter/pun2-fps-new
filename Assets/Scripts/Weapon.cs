using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Weapon : MonoBehaviourPun
{
    public GameObject healthPrefab;
    public GameObject wordPrefab;
    RaycastHit hit;
    public Gun[] loadout;
    //public GameObject[] loadout;
    public Transform weaponParent;

    public GameObject bulletPrefab;
    public LayerMask canbeShoot;
    public AudioSource soundEffect;

    private int currentIndex;
    private GameObject currentEquip;
    private bool isReloading;

    private void Start()
    {
        foreach(Gun g in loadout)
        {
            g.Init();
            //Equip(0);
        }
    }

    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1))
        {
            photonView.RPC("Equip", RpcTarget.All, 0);
        }

        if (currentEquip != null)
        {
            Aim((Input.GetMouseButton(0)));

            if (photonView.IsMine&& Input.GetKeyDown(KeyCode.J))
            {
                if (loadout[currentIndex].FireBullet())
                    photonView.RPC("Shoot", RpcTarget.All);
                else StartCoroutine(Reload(loadout[currentIndex].reloadTime));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                //StartCoroutine(Reload(loadout[currentIndex].reloadTime));
                photonView.RPC("ReloadRPC", RpcTarget.All);
            }

        }
    }

    public void RefreshAmmo(Text  p_text)
    {

        int clip = loadout[currentIndex].GetClip();
        int stash = loadout[currentIndex].GetStash();
        //Debug.Log(".......");
        //Debug.Log("clip: " + clip);
        //Debug.Log("stash: " + stash);
        p_text.text = clip.ToString() + " / " + stash.ToString();
    }

    [PunRPC]
    public void ReloadRPC()
    {
        StartCoroutine(Reload(loadout[currentIndex].reloadTime));
    }

    IEnumerator Reload(int wait)
    {
        isReloading = true;
        if (currentEquip.GetComponent<Animator>())
            currentEquip.GetComponent<Animator>().Play("Reload", 0, 0);//layer time
        else
            currentEquip.SetActive(false);

        //currentEquip.SetActive(false);

        yield return new WaitForSeconds(wait);

        loadout[currentIndex].Reload();
        currentEquip.SetActive(true);
        isReloading = false;
    }

    private void Aim(bool isAiming)
    {
        if (currentEquip == null)
        {
            return;
        }
        Transform anchor = currentEquip.transform.Find("Anchor");
        Transform state_ads = currentEquip.transform.Find("States/ADS");
        Transform state_hip = currentEquip.transform.Find("States/Hip");
        if (isAiming)
        {
            //Debug.Log("isAming.......");
            anchor.position = Vector3.Lerp(anchor.position, state_ads.position, loadout[currentIndex].aimSpeed);
        }
        else
        {
            //Debug.Log("not isAming.......");
            anchor.position = Vector3.Lerp(anchor.position, state_hip.position, loadout[currentIndex].aimSpeed);
        }
    }

    [PunRPC]
    void Equip(int weaponIndex)
    {
        if (currentEquip != null)
        {
            //avoid error ,when reloading and switch weapon;
            if(isReloading)
            StopCoroutine("Reload");
            Destroy(currentEquip);
        }
        currentIndex = weaponIndex;

        GameObject newEquip = Instantiate(loadout[weaponIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquip.transform.localPosition = Vector3.zero;//相对于自己的父物体，他的 transform 地址
        newEquip.transform.localEulerAngles = Vector3.zero;//rotation 相对于自己的父物体

        newEquip.GetComponent<Animator>().Play("Equip", 0, 0);

        currentEquip = newEquip;

    }
    [PunRPC]
    void Shoot()
    {
        Transform spawn = transform.Find("Body/Eyes/Camera");
        hit = new RaycastHit();
        if(Physics.Raycast(spawn.position,spawn.forward,out hit, 1000f, canbeShoot))
        {
            GameObject newHole =Instantiate(bulletPrefab, hit.point + hit.normal * 0.004f, Quaternion.identity);
            newHole.transform.LookAt(hit.normal);
            Destroy(newHole, 0.5f);
           
            //Debug.Log(hit.collider.gameObject.layer);
            //Debug.Log(hit.collider.gameObject.name);
            if (photonView.IsMine)
            {
                if (hit.collider.gameObject.layer == 11)//player
                {
                    //damage
                    hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    //Debug.Log("hit your enemy, damage it");
                    //hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    //send message to every machine

                    SpawnHealthBonus();
                    SpawnWordBonus();
                }
                if (hit.collider.gameObject.layer==10)//enemy
                {
                    Debug.Log("hit layer 10");
                    PhotonNetwork.Destroy(hit.collider.gameObject);
                }

                //if (hit.collider.gameObject.layer == 13)
                //{
                //    PhotonNetwork.Destroy(hit.collider.gameObject);
                //    Debug.Log("kill enemy");
                //}
            }
        }
        soundEffect.Stop();
        soundEffect.clip = loadout[currentIndex].gunShot;
        soundEffect.pitch = 1 - loadout[currentIndex].shotPitch * Random.Range(-loadout[currentIndex].shotPitch, loadout[currentIndex].shotPitch);
        soundEffect.Play();
    }


    [PunRPC]
    private void TakeDamage(int damage)//every machine receive message from rpc
    {
        //Debug.Log("this is weapon rpc,get motion component");
        GetComponent<Motion>().TakeDamage(damage);
        //then get Motion component,call it's public method
    }


    public void SpawnHealthBonus()
    {
        GameObject obj= Instantiate(healthPrefab, hit.collider.gameObject.transform.position, Quaternion.identity);
        Destroy(obj, 3);
    }

    public void SpawnWordBonus()
    {
        Vector3 randPos = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        GameObject obj = Instantiate(wordPrefab, hit.collider.gameObject.transform.position + randPos, Quaternion.identity);
        Word temp = WordGenerator.GetSingleWord();

        obj.transform.Find("Canvas/wordId").GetComponent<Text>().text = temp.WordId.ToString();
        obj.transform.Find("Canvas/spell").GetComponent<Text>().text = temp.Spell;
        obj.transform.Find("Canvas/explaination").GetComponent<Text>().text = temp.Explaination;
        Debug.Log("temp word is " + temp.Spell);
        Destroy(obj, 3);
    }

}
