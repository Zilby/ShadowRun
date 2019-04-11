using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;
using OptionData = TMPro.TMP_Dropdown.OptionData;

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

    private string skill;
    [Binding]
    public string Skill
    {
        get { return skill; }
        set { SetProperty(ref skill, value, nameof(Skill)); }
    }

    [Binding]
    public List<string> SkillOptions { get { return CharacterSheetPanel.SkillOptions; } }

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
    public void SelectOption()
    {
        DropdownWindow.ShowDropdown(SkillOptions, option =>
        {
            Skill = option;
            Debug.Log($"option set to {option}");
        },
        SkillOptions.IndexOf(Skill));
    }

    [Binding]
    public void CreateTest()
    {
        FeedModel.Instance.AddMessage("GM",
        $"Created new success test for {Skill}",
        new TestData
        {
            PlayerSkill = Skill,
            SkillThreshold = Threshold
        },
        send: true);
        Debug.Log($"New test for {Skill} at threshold {Threshold}");
        PanelStack.Instance.PopPanel();
        NotificationSystem.DisplayNotification("Test Created");
    }
}
