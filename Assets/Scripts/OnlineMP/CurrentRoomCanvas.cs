using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingMenu _playerListingMenu;
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;

    private RoomsCanvasManager _roomsCanvases;

    public void Initialize(RoomsCanvasManager canvases)
    {
        _roomsCanvases = canvases;
        _playerListingMenu.Initialize(canvases);
        _leaveRoomMenu.Initialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
