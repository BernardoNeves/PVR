using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }

    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }

}
