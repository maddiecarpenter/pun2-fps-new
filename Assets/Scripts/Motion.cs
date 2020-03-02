using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Motion : MonoBehaviourPunCallbacks
{
    public float speed=200f;
    public int max_health;
    public GameObject camParent;
    private Transform ui_healthBar;
    private static Text ui_ammo;
    private Rigidbody rig;
    private int current_health;
    private GameManaging manager;
    private Weapon weapon;

    private bool isProtected;
    private float defendTime;
    public void Start()
    {
        weapon = GetComponent<Weapon>();
        manager = GameObject.Find("Manager").GetComponent<GameManaging>();
        current_health = max_health;

        camParent.SetActive(photonView.IsMine);
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
            
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //if (Input.GetKeyDown(KeyCode.U)) TakeDamage(50);
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
        ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(ratio, 1, 1), Time.deltaTime * 8f);
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            Debug.Log("you are shooted by your enemy, damage= " + damage);
            current_health -= damage;
            Debug.Log("current_health: "+ current_health);
            RefreshBar();
            //Debug.Log("current health is " + current_health);
            if (current_health <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
                manager.Spawn(3);
            }
        }
    }

}
