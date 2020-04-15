using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DestroyBonus : MonoBehaviourPun
{
    //属于奖励 bonus 的组件，使用协程在生成后的 15s 后自动销毁
    private void Start()
    {
        StartCoroutine(DestroyAutoAfterSeconds());
    }

    [PunRPC]
    public void DestroyAuto()
    {
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("destroy the bonus auto");
    }

    IEnumerator DestroyAutoAfterSeconds()
    {
        yield return new WaitForSeconds(15);
        PhotonNetwork.Destroy(gameObject);
        GameManaging.alertText.text = "destroy bonus after 10 seconds";
    }

    //碰撞到 layer 为 localplayer 的，自动销毁
    private void OnTriggerEnter(Collider other)
    {
        GameManaging.alertText.text = "destroy bonus";
        if (other.gameObject.layer == 9)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
