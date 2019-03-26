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

    private ObservableList<FeedMessageView> messages;
    [Binding]
    public ObservableList<FeedMessageView> Messages
    {
        get { return messages; }
        private set { SetProperty(ref messages, value, nameof(Messages)); }
    }

    private void Start()
    {
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
    public void GoToTestMaker()
    {
        PanelStack.Instance.PushPanel<TestMakerPanel>();
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        Canvas.ForceUpdateCanvases();
        scrollview.enabled = false;
        scrollview.enabled = true;
    }
}
