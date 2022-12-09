using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class MultiPlayerLevelManager : MonoBehaviourPunCallbacks
{
    public int maxKills = 3;
    public GameObject gameOverPopup;
    public Text winnerText;

    private void Start()
    {
        PhotonNetwork.Instantiate("MultiPlayer Wolve", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == maxKills)
        {
            winnerText.text = targetPlayer.NickName;
            gameOverPopup.SetActive(true);
        }

        StorePersonalBest();
    }

    void StorePersonalBest()
    {
        int currentScore = PhotonNetwork.LocalPlayer.GetScore();
        PlayerData playerData = GameManager.shared.playerData;

        if (currentScore > playerData.bestScore)
        {
            playerData.username = PhotonNetwork.LocalPlayer.NickName;
            playerData.bestScore = currentScore;
            playerData.bestScoreDate = DateTime.UtcNow.ToString();
            playerData.totalPlayersInGame = PhotonNetwork.CurrentRoom.PlayerCount;
            playerData.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManager.shared.SavePlayerData();
        }
    }

    public void LeaveGame()
    {
        Debug.Log("LEAVE BUTTON PRESSED");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("MultiPlayer");
    }
}

