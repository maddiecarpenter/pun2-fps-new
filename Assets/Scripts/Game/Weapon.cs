using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Weapon : MonoBehaviourPun
{
    //预制体
    public GameObject healthPrefab;
    public GameObject wordPrefab;
    public GameObject bulletPrefab;
    //射线检测
    RaycastHit hit;
    //脚本化物体
    public Gun[] loadout;
    //public GameObject[] loadout;
    //玩家预制体中武器的父物体
    public Transform weaponParent;
    //inspector 设置不同的 layer， localplayer 9 ,enemy 10,player 11
    //设置子弹可以射击的 layer
    public LayerMask canbeShoot;
    //枪击声音来源
    public AudioSource soundEffect;
    //当前枪支的 index
    private int currentIndex;
    //当前枪支物体
    private GameObject currentEquip;
    //是否在装载子弹中
    private bool isReloading;

    private void Start()
    {
        foreach(Gun g in loadout)
        {
            g.Init();
        }
    }

    private void Update()
    {
        //装备枪支
        if (photonView.IsMine)
        {
            //检测按钮，选择武器
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("Equip", RpcTarget.All, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("Equip", RpcTarget.All, 1);
            }
        }
        //如果当前有武器
        if (currentEquip != null)
        {
            //Input.GetMouseButton(0)检测到了鼠标左键
            Aim((Input.GetMouseButton(0)));

            //j按钮作为射击键
            if (photonView.IsMine&& Input.GetKeyDown(KeyCode.J))
            {
                //判断是否还有子弹
                if (loadout[currentIndex].FireBullet())
                    photonView.RPC("Shoot", RpcTarget.All);
                ////没有子弹就花费几秒钟装载子弹
                //else StartCoroutine(Reload(loadout[currentIndex].reloadTime));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload(loadout[currentIndex].reloadTime));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FillBullets")
        {
            Debug.Log("fill your bullets");
            GameManaging.alertText.text="refill your bullets compelete";
            loadout[currentIndex].SetClip();
            loadout[currentIndex].SetStash();
        }
    }

    //更新子弹
    public void RefreshAmmo(Text  p_text)
    {
        int clip = loadout[currentIndex].GetClip();
        int stash = loadout[currentIndex].GetStash();
        //Debug.Log("clip: " + clip);
        //Debug.Log("stash: " + stash);
        p_text.text = clip.ToString() + " / " + stash.ToString();
    }

    //协程，等待动画执行完毕之后才更新子弹
    IEnumerator Reload(int wait)
    {
        isReloading = true;
        if (currentEquip.GetComponent<Animator>())
            currentEquip.GetComponent<Animator>().Play("Reload", 0, 0);//layer 0 ,start time 0s
        else
            currentEquip.SetActive(false);
        yield return new WaitForSeconds(wait);

        loadout[currentIndex].Reload();
        currentEquip.SetActive(true);
        isReloading = false;
    }

    private void Aim(bool isAiming)
    {
        //如果此时的武器不存在
        if (currentEquip == null)
        {
            GameManaging.alertText.text = "current equip is null";
            return;
        }

        Transform anchor = currentEquip.transform.Find("Anchor");
        //瞄准状态下枪支位置
        Transform state_ads = currentEquip.transform.Find("States/ADS");
        //正常状态下枪支位置
        Transform state_hip = currentEquip.transform.Find("States/Hip");
        if (isAiming)
        {
            anchor.position = Vector3.Lerp(anchor.position, state_ads.position,Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, state_hip.position,Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
    }

    [PunRPC]
    void Equip(int weaponIndex)
    {
        //如果已经有了枪支，就把枪支毁掉，装备上新枪支
        if (currentEquip != null)
        {
            Destroy(currentEquip);
        }
        currentIndex = weaponIndex;

        GameObject newEquip = Instantiate(loadout[weaponIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquip.transform.localPosition = Vector3.zero;//相对于自己的父物体，他的 transform 地址
        newEquip.transform.localEulerAngles = Vector3.zero;//rotation 相对于自己的父物体

        //枪支动画播放
        newEquip.GetComponent<Animator>().Play("Equip", 0, 0);

        currentEquip = newEquip;
    }

    [PunRPC]
    void Shoot()
    {
        //玩家身上的相机，作为眼睛
        Transform spawn = transform.Find("Body/Eyes/Camera");
        hit = new RaycastHit();
        //canbeShoot是可以射击的 layer,在 inspector 中设置
        if (Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canbeShoot))
        {
            //hit.normal 的方向，垂直于被击中的面
            GameObject newHole =Instantiate(bulletPrefab, hit.point + hit.normal * 0.004f, Quaternion.identity);
            //newHole.transform.LookAt(hit.normal);
            Destroy(newHole, 0.5f);

            if (photonView.IsMine)
            {
                if (hit.collider.gameObject.layer == 11)//player
                {
                    //damage
                    hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    //send message to every machine
                    Debug.Log("hit.collider.GetComponent<Motion>().current_health is --" + hit.collider.GetComponent<Motion>().current_health);
                }
                LayerMask mask = LayerMask.NameToLayer("enemy");
                if (hit.collider.gameObject.layer == mask.value)//enemy 10
                {
                    hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    SpawnHealthBonus();
                    SpawnWordBonus();
                }
            }
            else
            {
                GameManaging.alertText.text = "photon view not mine !";
            }
        }
        //soundEffect.Stop();
        soundEffect.clip = loadout[currentIndex].gunShot;
        soundEffect.pitch = 1 - loadout[currentIndex].shotPitch * Random.Range(-loadout[currentIndex].shotPitch, loadout[currentIndex].shotPitch);
        soundEffect.Play();
    }

    [PunRPC]
    private void TakeDamage(int damage)//每个机器都 receive message from rpc
    {
        GetComponent<Motion>().TakeDamage(damage);
    }

    //产生生命值的奖励
    public void SpawnHealthBonus()
    {
        GameObject obj = PhotonNetwork.Instantiate(healthPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity);
    }

    //产生单词奖励
    public void SpawnWordBonus()
    {
        Vector3 randPos = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        GameObject obj = PhotonNetwork.Instantiate(wordPrefab.name, hit.collider.gameObject.transform.position + randPos, Quaternion.identity);
        Word temp = WordGenerator.GetSingleWord();

        obj.transform.Find("Canvas/wordId").GetComponent<Text>().text = temp.WordId.ToString();
        obj.transform.Find("Canvas/spell").GetComponent<Text>().text = temp.Spell;
        obj.transform.Find("Canvas/explaination").GetComponent<Text>().text = temp.Explaination;
    }
}
