using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Motion : MonoBehaviourPunCallbacks
{
    public Text alertTxt;

    public float speed=200f;
    public int max_health;
    public GameObject camParent;
    private Transform ui_healthBar;
    private static Text ui_ammo;
    private Rigidbody rig;
    private int current_health;
    private GameManaging manager;
    public Text name;
    private Weapon weapon;
    private bool isProtected;
    private float defendTime;
   
    public void Start()
    {
        alertTxt= GameObject.Find("Canvas/Hud/Health/Text").GetComponent<Text>();
        weapon = GetComponent<Weapon>();
        manager = GameObject.Find("Manager").GetComponent<GameManaging>();
        current_health = max_health;
        camParent.SetActive(photonView.IsMine);
        name.text = Launcher.name;
        if (!photonView.IsMine)
        {
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
            ui_healthBar = GameObject.Find("Hud/Health/Bar").transform;
            ui_ammo = GameObject.Find("Hud/Ammo/Text").GetComponent<Text>();
            RefreshBar();
            Debug.Log("refresh");
            Debug.Log("ratio is " + (float)current_health / (float)max_health);
        }
        isProtected = true;
        defendTime = 5;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="HealthBonus")
        {
            if(current_health<=max_health-10)
                current_health += 10;
            Destroy(other.gameObject);
            Debug.Log("add health");
        }
        if (other.gameObject.tag == "WordBonus")
        {
            Debug.Log("add word to list");
            //Debug.Log(other.transform.Find("Canvas/wordId").GetComponent<Text>().text);
            Word tempWord = new Word();
            tempWord.WordId = int.Parse(other.transform.Find("Canvas/wordId").GetComponent<Text>().text);
            tempWord.Spell = other.transform.Find("Canvas/spell").GetComponent<Text>().text;
            tempWord.Explaination = other.transform.Find("Canvas/explaination").GetComponent<Text>().text;

            WordGenerator.list.Add(tempWord);

        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.U)) TakeDamage(20);
        if (isProtected)
        {
            defendTime -= Time.deltaTime;
            if (defendTime <= 0)
            {
                isProtected = false;
            }
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
        rig.velocity = transform.TransformDirection(vector3*speed*Time.deltaTime);
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
            Debug.Log("you are shooted by your enemy, damage= " + damage);

            current_health -= damage;
            string info = "you are shooted by your enemy, damage= " + damage + ", current health " + current_health;
            AlertText(info);

            Debug.Log("current_health: "+ current_health);
            Debug.Log("ratio is " + (float)current_health / (float)max_health);
            RefreshBar();
            if (current_health <= 0)
            {
                //photonView.RPC("SpawnWordBonus", RpcTarget.All);
                //photonView.RPC("SpawnHealthBonus", RpcTarget.All);
                PhotonNetwork.Destroy(gameObject);
                manager.Spawn(3);
            }
        }
    }

    public void AlertText(string info)
    {
        GameObject obj = Instantiate
        (
        (GameObject)Resources.Load("AlertText"),
        //Vector3.zero, Quaternion.identity,
        GameObject.Find("Canvas/Hud").transform
        );
        obj.GetComponentInChildren<Text>().text = info;
        Destroy(obj,.5f);

    }


}
