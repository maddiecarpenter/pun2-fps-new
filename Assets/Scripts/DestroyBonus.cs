using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DestroyBonus : MonoBehaviourPun
{
    [PunRPC]
    public void DestroyAuto()
    {
        Debug.Log("get your bonus");
        PhotonNetwork.Destroy(gameObject);
    }
}
