using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviourPun
{
    public float speed = 200f;
    public int max_health=100;
    //预制体的一部分，可以显示敌人的生命值
    private Transform ui_healthBar;
    private int current_health;
    GameManaging manager;
    //控制敌人自动追踪玩家的关键；
    public NavMeshAgent agent;
    //随时获取玩家的位置；
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)//check on localPlayer 
        {
            other.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, 10);
            //检测到敌人和玩家碰撞时候，敌人颜色变成红色
            transform.Find("Body/Body").GetComponent<MeshRenderer>().material.color = Color.red;
            transform.Find("Body/Glass").GetComponent<MeshRenderer>().material.color = Color.red;

            GameManaging.alertText.text = "enemy hit local player";
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //敌人和玩家分离的时候，玩家变成正常颜色
        transform.Find("Body/Body").GetComponent<MeshRenderer>().material.color = Color.white;
        transform.Find("Body/Glass").GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManaging>();
        current_health = max_health;
        ui_healthBar = transform.Find("Canvas/Health/Bar").transform;
    }

    private void FixedUpdate()
    {
        //如果玩家在 navmesh 上面，同时能够得知场景中玩家位置
        if (agent.isOnNavMesh && player)
        {
            agent.SetDestination(player.transform.position);
        }
        else if(agent.isOnNavMesh!=true)
        {
            Debug.Log("agent is not on navmesh");
        }
        else
        {
            agent.SetDestination(Vector3.zero);
        }
        RefreshBar();

    }
    private void Update()
    {
        //实时更新玩家的信息
        player = GameObject.FindGameObjectWithTag("localplayer");
    }

    void RefreshBar()
    {
        //更新血条的信息
        float ratio = (float)current_health / (float)max_health;
        //ui_healthBar.localScale = new Vector3(ration, 1, 1);
        //lerp 血条减少是渐变的，可以设置时间
        ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(ratio, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        current_health -= damage;
        Debug.Log("enemy current health is " + current_health);
        if (current_health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
            manager.SpawnEnemy();
        }
    }
}
