using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class FeedMessageView : DataBindObject
{
    private string diceRoll;
    [Binding]
    public string DiceRoll
    {
        get { return diceRoll; }
        private set { SetProperty(ref diceRoll, value, nameof(DiceRoll)); }
    }

    private string sender;
    [Binding]
    public string Sender
    {
        get { return sender; }
        private set { SetProperty(ref sender, value, nameof(Sender)); }
    }

    private string text;
    [Binding]
    public string Text
    {
        get { return text; }
        private set { SetProperty(ref text, value, nameof(Text)); }
    }

    private bool isTest;
    [Binding]
    public bool IsTest
    {
        get { return isTest; }
        set { SetProperty(ref isTest, value, nameof(IsTest)); }
    }

    public FeedMessageView(string sender, string text, bool isTest)
    {
        Sender = sender;
        Text = text;
        IsTest = isTest;
    }

    [Binding]
    public void LinkToDiceRoller()
    {
        PanelStack.Instance.PushPanel<DiceRollPanel>();
    }
}
