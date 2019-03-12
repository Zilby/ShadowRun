using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class FeedModel : DataBindObject
{
    private static FeedModel instance;
    public static FeedModel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FeedModel();
            }
            return instance;
        }
    }

    private ObservableList<FeedMessageView> messages = new ObservableList<FeedMessageView>();
    [Binding]
    public ObservableList<FeedMessageView> Messages
    {
        get { return messages; }
        set { SetProperty(ref messages, value, nameof(Messages)); }
    }

    public void AddMessage(string sender, string text)
    {
        Messages.Add(new FeedMessageView(sender, text));
    }

    private FeedModel() { }
}
