using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//负责场景二，游戏运行之时的管理
public class GameManaging : MonoBehaviour
{
    //作为提示文字
    public static Text alertText;
    //在 inspector 中填写预制体的名字，一般存放在 resource 中
    public string playerPrefab;
    public string enemyPrefab;
    //玩家或者敌人预制体生成的地点
    public Transform []spawnPoints;
    Transform spawn;
    //菜单
    public GameObject menu;
    //控制菜单的显示
    private bool isMenuActive = false;
    //帮助界面
    public GameObject help;

    private void Start()
    {
        alertText = GameObject.Find("Canvas/Hud/AlertText/AlertText").GetComponent<Text>();
        alertText.text = "Alert info";
        menu.SetActive(isMenuActive);
        //游戏一开始初始化玩家和敌人
        Spawn(3);
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

    public void Spawn(int wait)
    {
        Debug.Log("spawn your player");
        spawn = spawnPoints[0];
        GameObject player = PhotonNetwork.Instantiate(playerPrefab, spawn.position, spawn.rotation);
    }

    public void SpawnEnemy()
    {
        Debug.Log("spawn enemy");
        //第一个 spawn 位置固定给了玩家
        spawn = spawnPoints[Random.Range(1, spawnPoints.Length)];
        GameObject enemy= PhotonNetwork.Instantiate(enemyPrefab, spawn.position, spawn.rotation);
    }

    private void  Update()
    {
        //检测到按了 ESC 按钮，可以控制菜单显示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            menu.SetActive(isMenuActive);
        }
    }

     //菜单上可点击的 button，跳转到学习页面Learn 2
    public void Learn()
    {
        SceneManager.LoadScene(2);
    }
    //菜单上的 button，跳转到登录注册页面，Menu 0
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
     //点击之后可以看到游戏介绍页面，在 5s 之后自动隐藏
    public void Help()
    {
        help.SetActive(true);
        StartCoroutine(WaitDestroy());
    }

    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(5);
        help.SetActive(false);
    }
}
