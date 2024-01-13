using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance { get; private set; }
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text roomName;
    [SerializeField] Transform roomListTransform;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Transform playerListTransform;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.EnableCloseConnection = true;
        MenuManager.Instance.OpenMenu("Loading");
        PhotonNetwork.NickName = "Player#" + Random.Range(0, 1000).ToString("0000");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Main");
    }

    public void OnPlayerChangeName()
    {
        if (playerNameInputField.text != "")
        {
            PhotonNetwork.NickName = playerNameInputField.text?.Substring(0, Mathf.Min(playerNameInputField.text.Length, 16));
            playerNameInputField.text = PhotonNetwork.NickName;
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            roomNameInputField.text = "Room#" + Random.Range(0, 1000).ToString("0000");
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");

    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.Instance.OpenMenu("Room");
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform transform in playerListTransform)
        {
            Destroy(transform.gameObject);
        }
        foreach (Player player in players)
        {
            Instantiate(playerPrefab, playerListTransform).GetComponent<PlayerListItem>().SetUp(player, PhotonNetwork.IsMasterClient);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        MenuManager.Instance.OpenMenu("Room");
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform transform in playerListTransform)
        {
            Destroy(transform.gameObject);
        }
        foreach (Player player in players)
        {
            Instantiate(playerPrefab, playerListTransform).GetComponent<PlayerListItem>().SetUp(player, PhotonNetwork.IsMasterClient);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.Instance.OpenMenu("Error");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in roomListTransform)
        {
            Destroy(transform.gameObject);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
                continue;
            Instantiate(roomPrefab, roomListTransform).GetComponent<RoomListItem>().SetUp(roomInfo);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Main");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListItem = Instantiate(playerPrefab, playerListTransform);
        playerListItem.GetComponent<PlayerListItem>().SetUp(newPlayer, PhotonNetwork.IsMasterClient);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
