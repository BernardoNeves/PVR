using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHostButtons : MonoBehaviour
{
    [SerializeField] Button giveHost;
    [SerializeField] Button kickPlayer;
    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        kickPlayer.onClick.AddListener(() =>
        {
            Player[] players = PhotonNetwork.PlayerList;
            string playerName = player.NickName;
            foreach (Player p in players)
            {
                if (p.NickName == playerName)
                {
                    PhotonNetwork.CloseConnection(p);
                    break;
                }
            }
        });
        giveHost.onClick.AddListener(() =>
        {
            Player[] players = PhotonNetwork.PlayerList;
            string playerName = player.NickName;
            foreach (Player p in players)
            {
                if (p.NickName == playerName)
                {
                    PhotonNetwork.SetMasterClient(p);
                    break;
                }
            }
        });
    }


}
