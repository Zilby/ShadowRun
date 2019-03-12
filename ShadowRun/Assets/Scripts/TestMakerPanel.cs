using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;

[Binding]
public class TestMakerPanel : Panel
{
    private bool success;
    [Binding]
    public bool Success
    {
        get { return success; }
        set { SetProperty(ref success, value, nameof(Success)); }
    }

    private Dropdown.OptionData skill;
    [Binding]
    public Dropdown.OptionData Skill
    {
        get { return skill; }
        set { SetProperty(ref skill, value, nameof(Skill)); }
    }

    [Binding]
    public List<Dropdown.OptionData> SkillOptions { get { return CharacterSheetPanel.SkillOptions; } }

    private int threshold;
    [Binding]
    public int Threshold
    {
        get { return threshold; }
        set { SetProperty(ref threshold, value, nameof(Threshold)); }
    }

    [Binding]
    public void IncrementThreshold()
    {
        Threshold++;
    }

    [Binding]
    public void DecrementThreshold()
    {
        Threshold--;
    }

    [Binding]
    public void SetOption(int option)
    {
        Skill = SkillOptions[option];
        Debug.Log($"option set to {option}");
    }

    [Binding]
    public void CreateTest()
    {
        FeedModel.Instance.AddMessage("GM", $"Created new success test for {Skill.text}", true);
        Debug.Log($"New test for {Skill.text} at threshold {Threshold}");
        PanelStack.Instance.PopPanel();
    }
}
