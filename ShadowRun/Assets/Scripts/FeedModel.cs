using System;
using System.Collections;
using System.Collections.Generic;
using SendBird;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class FeedModel : DataBindObject, IDisposable
{
    private static FeedModel instance;
    public static FeedModel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FeedModel();
                instance.Init();
            }
            return instance;
        }
    }

    private MonoHost coroutineHost;

    private ObservableList<FeedMessageView> messages = new ObservableList<FeedMessageView>();
    [Binding]
    public ObservableList<FeedMessageView> Messages
    {
        get { return messages; }
        set { SetProperty(ref messages, value, nameof(Messages)); }
    }

    public string RoomCode { get; private set; }
    private SendBirdClient.ChannelHandler channelHandler;
    private string chid;
    private OpenChannel channel;

    private void Init()
    {
        coroutineHost = new GameObject().AddComponent<MonoHost>();
        SendBirdClient.SetupUnityDispatcher(coroutineHost.gameObject);
        coroutineHost.StartCoroutine(SendBirdClient.StartUnityDispatcher);

        SendBirdClient.Init("71CAF499-268F-49A3-A2F5-9DF09F275FB8");

        channelHandler = new SendBirdClient.ChannelHandler();
        chid = channelHandler.GetHashCode().ToString();
        channelHandler.OnMessageReceived += ChannelHandler_OnMessageRecieved;
        SendBirdClient.AddChannelHandler(chid, channelHandler);
    }

    public void Dispose()
    {
        SendBirdClient.RemoveChannelHandler(chid);
        channelHandler.OnMessageReceived -= ChannelHandler_OnMessageRecieved;
    }

    public void Connect(Action onConnected)
    {
        if (!PlayerPrefs.HasKey("USER_ID"))
        {
            PlayerPrefs.SetString("USER_ID", Guid.NewGuid().ToString());
        }
        var userID = PlayerPrefs.GetString("USER_ID");

        SendBirdClient.Connect(userID, (User user, SendBirdException e) =>
        {
            if (e != null)
            {
                Debug.LogError(e);
                return;
            }
            onConnected?.Invoke();
        });
    }

    public void Disconnect()
    {
        SendBirdClient.Disconnect(() =>
        {

        });
    }

    public void CreateChannel(string name, Action<OpenChannel, Exception> onChannelCreated)
    {
        var code = Math.Abs(name.GetHashCode()).ToString().Substring(0, 6);

        OpenChannel.CreateChannel(name, null, code.ToString(), (channel, e) =>
        {
            if (e != null)
            {
                Debug.LogError(e);
            }
            else
            {
                RoomCode = channel.Data;
            }
            onChannelCreated?.Invoke(channel, e);
        });
    }

    public void EnterChannel(string name, Action<OpenChannel, Exception> onChannelEntered)
    {
        GetChannelFromList(name, (channel, e) =>
        {
            if (e != null)
            {
                Debug.LogError(e);
                onChannelEntered?.Invoke(null, e);
                return;
            }
            channel.Enter(e2 =>
            {
                if (e2 != null)
                {
                    Debug.LogError(e2);
                }
                this.channel = channel;
                onChannelEntered?.Invoke(channel, e2);
            });
        });
    }

    private void GetChannelFromList(string channelMetadata, Action<OpenChannel, SendBirdException> resultHandler)
    {
        OpenChannelListQuery query = OpenChannel.CreateOpenChannelListQuery();
        query.Next((channels, e) =>
        {
            if (e != null)
            {
                Debug.LogError(e);
                resultHandler?.Invoke(null, e);
                return;
            }
            var channel = channels.Find(c => c.Data == channelMetadata);
            resultHandler?.Invoke(channel, e);
        });
    }

    public void SendMessage(string message, ExtraData testData = null)
    {
        channel.SendUserMessage(message, testData.ToString(), (sentMessage, e) =>
        {
            if (e != null)
            {
                Debug.LogError(e);
                // Should also make notification
                return;
            }
        });
    }

    private void ChannelHandler_OnMessageRecieved(BaseChannel baseChannel, BaseMessage baseMessage)
    {
        var userMessage = baseMessage as UserMessage;
        if (userMessage != null)
        {
            var data = ExtraData.FromString(userMessage.Data);
            FeedModel.Instance.AddMessage(data.Sender, userMessage.Message, data.TestData);
        }
        var adminMessage = baseMessage as AdminMessage;
        if (adminMessage != null)
        {
            FeedModel.Instance.AddMessage("Admin", adminMessage.Message);
        }
    }

    public void AddMessage(string sender, string text, TestData testData = null, bool send = false)
    {
        Messages.Add(new FeedMessageView(sender, text, testData));
        if (send)
        {
            SendMessage(text, new ExtraData
            {
                Sender = sender,
                TestData = testData
            });
        }
    }

    private FeedModel() { }
}
