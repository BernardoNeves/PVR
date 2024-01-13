using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
            CreateController();
    }

    private void CreateController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), new Vector3(Random.Range(-2.5f, 2.5f), 2, Random.Range(-2.5f, 2.5f)), Quaternion.identity, 0, new object[] { PV.ViewID });
    }

    private void DestroyController()
    {
        PhotonNetwork.Destroy(controller);
    }

    public void Death()
    {
        DestroyController();
        CreateController();
    }
}
