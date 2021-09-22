using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoom _createRoom;
    [SerializeField]
    private RoomListingsMenu _roomListingsMenu;

    private RoomsCanvasManager _roomsCanvases;

    public void Initialize(RoomsCanvasManager canvases)
    {
        _roomsCanvases = canvases;
        _createRoom.Initialize(canvases);
        _roomListingsMenu.Initialize(canvases);
    }

}
