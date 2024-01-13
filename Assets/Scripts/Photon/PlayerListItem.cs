using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] GameObject HostButtons;
    Player player;

    public void SetUp(Player _player, bool showHostButtons)
    {
        player = _player;
        playerName.text = player.NickName;

        if (player.IsLocal)
        { 
            playerName.fontStyle = FontStyles.Bold;
            playerName.text += "(You)";
        }
        else if (player.IsMasterClient)
        {
            playerName.text += "(Host)";
        }
        HostButtons.SetActive(showHostButtons && !player.IsLocal);
        if (showHostButtons && !player.IsLocal) HostButtons.GetComponent<PlayerHostButtons>().SetUp(_player);
    }

    public override void OnPlayerLeftRoom(Player _player)
    {
        if (player == _player)
            Destroy(gameObject);
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
