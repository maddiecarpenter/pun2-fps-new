using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManaging : MonoBehaviour
{
    public static Text alertText;

    public GameObject wordBonusList;
    public string playerPrefab;
 
    public Transform []spawnPoints;
    Transform spawn;
    public Transform menu;
    private bool isActive = false;
    private void Start()
    {
        alertText = GameObject.Find("Canvas/Hud/AlertText/AlertText").GetComponent<Text>();
        alertText.text = "Alert info";
        menu.gameObject.SetActive(isActive);
        wordBonusList.SetActive(isActive);
        //StartCoroutine(Spawn(3));
        Spawn(3);

    }

    public void Spawn(int wait)
    {
        Debug.Log("spawn your player");
        spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(playerPrefab, spawn.position, spawn.rotation);

    }


    private void  Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            menu.gameObject.SetActive(isActive);
            wordBonusList.SetActive(isActive);
        }
    }

    public void Learn()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }


}
