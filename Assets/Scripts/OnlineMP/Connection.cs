using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        print("Connecting to Photon...");
        PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to Photon. Nickname: " + PhotonNetwork.NickName);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server. Photon: " + cause.ToString());
    }
}
