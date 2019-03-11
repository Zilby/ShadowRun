using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStack : MonoBehaviour
{
    public enum PanelChangeType
    {
        PUSH,
        POP,
        CLEAR
    }

    public class PanelChangedEventArgs : EventArgs
    {
        public PanelChangeType ChangeType { get; set; }
        public Panel OldPanel { get; set; }
        public Panel NewPanel { get; set; }
    }

    public event EventHandler<PanelChangedEventArgs> PanelChanged;

    [SerializeField]
    private Panel basePanel;

    private List<Panel> panels = new List<Panel>();
    public List<Panel> Panels
    {
        get { return panels; }
        set { panels = value; }
    }

    private void Awake()
    {
        if (basePanel == null)
        {
            Debug.LogError("PanelStack must have a base panel defined");
            return;
        }
        Panels.Add(basePanel);
        basePanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Adds a panel on top of the stack
    /// </summary>
    /// <param name="panel"></param>
    public void PushPanel(Panel panel)
    {
        if (panel == null)
        {
            Debug.LogError("Cannot push a null panel");
            return;
        }
        var oldPanel = Panels[Panels.Count - 1];
        oldPanel.gameObject.SetActive(false);
        Panels.Add(panel);
        var newPanel = Panels[Panels.Count - 1];
        newPanel.gameObject.SetActive(true);
        PanelChanged?.Invoke(this, new PanelChangedEventArgs { OldPanel = oldPanel, NewPanel = newPanel, ChangeType = PanelChangeType.PUSH });
    }

    /// <summary>
    /// Removes a panel from the stack
    /// </summary>
    public void PopPanel()
    {
        if (Panels.Count <= 1)
        {
            Debug.LogError("Cannot pop, must be showing at least one panel");
            return;
        }
        var oldPanel = Panels[Panels.Count - 1];
        oldPanel.gameObject.SetActive(false);
        Panels.RemoveAt(Panels.Count - 1);
        var newPanel = Panels[Panels.Count - 1];
        newPanel.gameObject.SetActive(true);
        PanelChanged?.Invoke(this, new PanelChangedEventArgs { OldPanel = oldPanel, NewPanel = newPanel, ChangeType = PanelChangeType.POP });
    }

    /// <summary>
    /// Removes all but the base panel from the stack
    /// </summary>
    public void ClearPanels()
    {
        if (Panels.Count <= 1)
        {
            Debug.Log("Already at base panel");
            return;
        }
        var oldPanel = Panels[Panels.Count - 1];
        Panels.RemoveRange(1, Panels.Count - 1);
        var newPanel = Panels[Panels.Count - 1];
        PanelChanged?.Invoke(this, new PanelChangedEventArgs { OldPanel = oldPanel, NewPanel = newPanel, ChangeType = PanelChangeType.CLEAR });
    }
}
