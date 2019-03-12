using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld;
using UnityWeld.Binding;

[Binding]
public class FeedPanel : Panel
{
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
    }

    [Binding]
    public void GoToCharacterPanel()
    {
        PanelStack.Instance.PushPanel<CharacterSheetPanel>();
    }
}
