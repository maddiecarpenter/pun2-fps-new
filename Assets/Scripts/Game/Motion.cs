using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Motion : MonoBehaviourPunCallbacks
{
    private Text healthText;
    public float speed=200f;
    public int max_health;
    public GameObject camParent;
    private Transform ui_healthBar;
    private static Text ui_ammo;
    private Rigidbody rig;
    public int current_health;
    private GameManaging manager;
    public Text name;
    private Weapon weapon;
    public AudioSource soundEffect;

    public void Start()
    {
        healthText = GameObject.Find("Canvas/Hud/Health/Text").GetComponent<Text>();
        weapon = GetComponent<Weapon>();
        manager = GameObject.Find("Manager").GetComponent<GameManaging>();
        current_health = max_health;
        camParent.SetActive(photonView.IsMine);
        name.text = Launcher.name;
        if (!photonView.IsMine)
        {
            GameManaging.alertText.text = "set other player 11";
            Debug.Log("set other player layer to player 11");
            foreach (Transform tran in GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = 11;
            }
        }

        //Camera.main.enabled = false;//可以访问物体
        //Camera.main.gameObject.SetActive(false);//勾选隐藏相机，但是不能访问物体
        rig = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            GameManaging.alertText.text = "bar setting for your self";
            ui_healthBar = GameObject.Find("Hud/Health/Bar").transform;
            ui_ammo = GameObject.Find("Hud/Ammo/Text").GetComponent<Text>();
            RefreshBar();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="HealthBonus")
        {
            if(current_health<=max_health-10)
                current_health += 10;
            //other.transform.root.gameObject.GetComponent<PhotonView>().RPC("AutoDestroy", RpcTarget.All);
        }

        if (other.gameObject.tag == "WordBonus")
        {
            //Debug.Log(other.transform.Find("Canvas/wordId").GetComponent<Text>().text);
            Word tempWord = new Word();
            tempWord.WordId = int.Parse(other.transform.Find("Canvas/wordId").GetComponent<Text>().text);
            tempWord.Spell = other.transform.Find("Canvas/spell").GetComponent<Text>().text;
            tempWord.Explaination = other.transform.Find("Canvas/explaination").GetComponent<Text>().text;

            Word word= WordGenerator.GetWordById(tempWord.WordId);
            WordGenerator.list.Add(tempWord);

            var distinct = WordGenerator.list.Distinct(new WordCompare());
            WordGenerator.list = distinct.ToList();
            GameManaging.alertText.text = "get word bonus";
            //other.gameObject.GetComponent<PhotonView>().RPC("AutoDestroy", RpcTarget.All);
            //other.transform.root.gameObject.GetPhotonView().RPC("AutoDestroy", RpcTarget.All);
            //Destroy(other.gameObject);
            //hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);

            //soundEffect.Stop();
            //soundEffect.clip = loadout[currentIndex].gunShot;
            soundEffect.pitch = 0.8f;
            soundEffect.Play();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        RefreshBar();
        weapon.RefreshAmmo(ui_ammo);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float hmove = Input.GetAxisRaw("Horizontal");
        float vmove = Input.GetAxisRaw("Vertical");
        Vector3 vector3=new Vector3(hmove,0, vmove);
        vector3.Normalize();
        rig.velocity = transform.TransformDirection(vector3 * speed * Time.deltaTime);
    }

    void RefreshBar()
    {
        float ratio = (float)current_health / (float)max_health;
        //ui_healthBar.localScale = new Vector3(ration, 1, 1);
        ui_healthBar.localScale = new Vector3(ratio, 1, 1);
        //ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(ratio, 1, 1), Time.deltaTime * 30f);
        GameObject.Find("Canvas/Hud/Health/Text").GetComponent<Text>().text = current_health + " / " + max_health;
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            current_health -= damage;
            string info = "you are shooted by your enemy, damage= " + damage + ", current health " + current_health;
            GameManaging.alertText.text = info;

            RefreshBar();
            if (current_health <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
                manager.Spawn(3);
            }
        }
    }

}
