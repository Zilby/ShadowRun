using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class FeedMessageView : DataBindObject
{
    private string sender;
    [Binding]
    public string Sender
    {
        get { return sender;}
        private set { SetProperty(ref sender, value, nameof(Sender));}
    }

    private string text;
    [Binding]
    public string Text
    {
        get { return text;}
        private set { SetProperty(ref text, value, nameof(Text));}
    }
    
    public FeedMessageView(string sender, string text)
    {
        Sender = sender;
        Text = text;
    }
}
