using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviourPun
{
    public GameObject healthPrefab;
    public GameObject wordPrefab;

    public float speed = 200f;
    public int max_health;
    private Transform ui_healthBar;
    private static Text ui_name;
    //private Rigidbody rig;
    private int current_health;
    GameManaging manager;
    Vector3 playerPos= Vector3.zero;
    public NavMeshAgent agent;
    private GameObject player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)//check on localPlayer 
        {
            other.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All,10);
            //foreach (Transform tran in GetComponentsInChildren<Transform>())
            //{
            //    tran.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            //}
            transform.Find("WholeBody/Body").GetComponent<MeshRenderer>().material.color = Color.red;
            transform.Find("WholeBody/Glass").GetComponent<MeshRenderer>().material.color = Color.red;

            Debug.Log("enemy attack you !!");

        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("on trigger exit");
        transform.Find("WholeBody/Body").GetComponent<MeshRenderer>().material.color = Color.white;
        transform.Find("WholeBody/Glass").GetComponent<MeshRenderer>().material.color = Color.white;
    }


    public void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManaging>();

        player = GameObject.FindGameObjectWithTag("localplayer");
        agent.SetDestination(player.transform.position);
        //playerPos = manager.spawnPoints[Random.Range(0, manager.spawnPoints.Length)].position;

        current_health = max_health;

        //if (!photonView.IsMine)
        //{
        //    gameObject.layer = 11;
        //    Debug.Log("set your enemy");
        //}

        //rig = GetComponent<Rigidbody>();
        //if (photonView.IsMine)
        //{
            ui_healthBar = transform.Find("Canvas/Health/Bar").transform;
            ui_name = transform.Find("Canvas/Name").GetComponent<Text>();
            RefreshBar();
        //}
    }

    private void FixedUpdate()
    {
        //if (!photonView.IsMine)
        //{
        //    Debug.Log("not mine");
        //    return;
        //}
        //float hmove = Input.GetAxisRaw("Horizontal");
        //float vmove = Input.GetAxisRaw("Vertical");
        //Vector3 vector3 = new Vector3(hmove, 0, vmove);
        //vector3.Normalize();
        //rig.velocity = transform.TransformDirection(vector3 * speed * Time.deltaTime);
        //if (Input.GetKeyDown(KeyCode.U)) TakeDamage(50);
        agent.SetDestination(player.transform.position);

    }
    private void Update()
    {
        RefreshBar();
    }

    void RefreshBar()
    {
        float ratio = (float)current_health / (float)max_health;
        //ui_healthBar.localScale = new Vector3(ration, 1, 1);
        ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(ratio, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        playerPos = manager.spawnPoints[Random.Range(0, manager.spawnPoints.Length)].position;

        //change word every time you hit it
        //Debug.Log("random word: " + WordGenerator.GetRandomWord());
        current_health -= damage;
        RefreshBar();
        //Debug.Log("current health is " + current_health);
        ui_name.text = current_health.ToString();
        if (current_health <= 0)
        {
            //PhotonNetwork.Instantiate(wordBonus, gameObject.transform.position, Quaternion.identity);
            //PhotonNetwork.Instantiate(healthBonus, gameObject.transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);

            SpawnWordBonus();
            SpawnHealthBonus();
            //manager.SpawnEnemy();
        }
    }


    public void SpawnHealthBonus()
    {
        Instantiate(healthPrefab, gameObject.transform.position, Quaternion.identity);
    }

    public void SpawnWordBonus()
    {
            Vector3 randPos = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
            GameObject obj = Instantiate(wordPrefab, gameObject.transform.position + randPos, Quaternion.identity);
            Word temp = WordGenerator.GetSingleWord();

            obj.transform.Find("Canvas/wordId").GetComponent<Text>().text = temp.WordId.ToString();
            obj.transform.Find("Canvas/spell").GetComponent<Text>().text = temp.Spell;
            obj.transform.Find("Canvas/explaination").GetComponent<Text>().text = temp.Explaination;
    }


}
