using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class LoginPanel : Panel
{
    private string roomName;
    [Binding]
    public string RoomName
    {
        get { return roomName; }
        set { SetProperty(ref roomName, value, nameof(RoomName)); }
    }

    private string roomCode;
    [Binding]
    public string RoomCode
    {
        get { return roomCode; }
        set { SetProperty(ref roomCode, value, nameof(RoomCode)); }
    }

    [Binding]
    public void CreateRoom()
    {
        RoomCode = RoomName.GetHashCode().ToString();
    }

    [Binding]
    public void EnterRoom()
    {
        PanelStack.Instance.PushPanel<FeedPanel>();
    }
}
