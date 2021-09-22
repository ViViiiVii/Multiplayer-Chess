using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{

    private RoomsCanvasManager _roomsCanvasManager;

    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _roomsCanvasManager.CurrentRoomCanvas.Hide();
    }

    public void Initialize(RoomsCanvasManager canvases)
    {
        _roomsCanvasManager = canvases;
    }
}
