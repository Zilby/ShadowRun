using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class Panel : DataBindMonobehaviour
{
    public virtual void Init(object args = null)
    {

    }

    [Binding]
    public void BackOut()
    {
        PanelStack.Instance.PopPanel();
    }
}
