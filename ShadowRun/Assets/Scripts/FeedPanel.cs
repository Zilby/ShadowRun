using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;

[Binding]
public class FeedPanel : Panel
{
    [SerializeField]
    LayoutGroup scrollview;

    private string roomCode;
    [Binding]
    public string RoomCode
    {
        get { return roomCode; }
        set { SetProperty(ref roomCode, value, nameof(RoomCode)); }
    }

    private ObservableList<FeedMessageView> messages;
    [Binding]
    public ObservableList<FeedMessageView> Messages
    {
        get { return messages; }
        private set { SetProperty(ref messages, value, nameof(Messages)); }
    }

    private void Start()
    {
        RoomCode = FeedModel.Instance.RoomCode;
        Messages = FeedModel.Instance.Messages;
        Messages.CollectionChanged += OnCollectionChanged;
    }

    void OnDestroy()
    {
        Messages.CollectionChanged -= OnCollectionChanged;
    }

    [Binding]
    public void GoToCharacterPanel()
    {
        PanelStack.Instance.PushPanel<CharacterSheetPanel>();
    }

    [Binding]
    public void GoToNPCPanel()
    {
        PanelStack.Instance.PushPanel<NPCPanel>();
    }

    [Binding]
    public void GoToTestMaker()
    {
        PanelStack.Instance.PushPanel<TestMakerPanel>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitThenFixCanvas());
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        if (enabled && gameObject.activeSelf)
        {
            StartCoroutine(WaitThenFixCanvas());
        }
    }

    private IEnumerator WaitThenFixCanvas()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        scrollview.enabled = false;
        scrollview.enabled = true;
    }
}
