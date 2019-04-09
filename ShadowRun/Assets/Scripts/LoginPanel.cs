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
        FeedModel.Instance.Connect(() =>
        {
            FeedModel.Instance.CreateChannel(RoomName, (channel, e) =>
            {
                if (e != null)
                {
                    Debug.LogError(e);
                    return;
                }
                RoomCode = channel.Data;
            });
        });
    }

    [Binding]
    public void EnterRoom()
    {
        FeedModel.Instance.Connect(() =>
        {
            FeedModel.Instance.EnterChannel(RoomCode, (channel, e) =>
            {
                if (e != null)
                {
                    Debug.LogError(e);
                    return;
                }
                PanelStack.Instance.PushPanel<FeedPanel>();
            });
        });
    }
}
