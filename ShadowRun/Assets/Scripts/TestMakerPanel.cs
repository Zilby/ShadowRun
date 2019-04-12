using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;
using OptionData = TMPro.TMP_Dropdown.OptionData;

[Binding]
public class TestMakerPanel : Panel
{
    private bool success = true;
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
        set
        {
            if (SetProperty(ref skill, value, nameof(Skill)))
            {
                OnPropertyChanged(nameof(TestReady));
            }
        }
    }

    private string opponentSkill;
    [Binding]
    public string OpponentSkill
    {
        get { return opponentSkill; }
        set
        {
            if (SetProperty(ref opponentSkill, value, nameof(OpponentSkill)))
            {
                OnPropertyChanged(nameof(TestReady));
            }
        }
    }

    private Character opponent;
    [Binding]
    public Character Opponent
    {
        get { return opponent; }
        set
        {
            if (SetProperty(ref opponent, value, nameof(Opponent)))
            {
                OnPropertyChanged(nameof(HasOpponent));
                OnPropertyChanged(nameof(TestReady));
                OnPropertyChanged(nameof(OpponentName));
            }
        }
    }

    [Binding]
    public string OpponentName => Opponent?.Name;

    [Binding]
    public bool HasOpponent => Opponent != null;

    [Binding]
    public bool TestReady
    {
        get
        {
            if (Success)
            {
                return Skill != null && CharacterModel.SkillNamesToRelatedAttrs.ContainsKey(Skill);
            }
            else
            {
                return Opponent != null &&
                    Skill != null &&
                    OpponentSkill != null &&
                    CharacterModel.SkillNamesToRelatedAttrs.ContainsKey(Skill) &&
                    CharacterModel.SkillNamesToRelatedAttrs.ContainsKey(OpponentSkill);
            }
        }
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

    public override void Init(object args = null)
    {
        Success = true;
        Skill = null;
        Opponent = null;
        OpponentSkill = null;
        Threshold = 0;
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
        DropdownWindow.ShowDropdown(
            SkillOptions,
            option => Skill = option,
            SkillOptions.IndexOf(Skill));
    }

    [Binding]
    public void SelectEnemy()
    {
        if (CharacterModel.Instance.Characters.NPCs.Count == 0)
        {
            NotificationSystem.DisplayNotification("You must create at least one NPC to create an opposed test.");
            return;
        }
        DropdownWindow.ShowDropdown(
            CharacterModel.Instance.Characters.NPCs.Select(c => c.Name).ToList(),
            npc => Opponent = CharacterModel.Instance.Characters.NPCs.Find(c => c.Name == npc),
            CharacterModel.Instance.Characters.NPCs.IndexOf(Opponent));
    }

    [Binding]
    public void SelectEnemySkill()
    {
        if (HasOpponent)
        {
            DropdownWindow.ShowDropdown(
                //Opponent.Skills.Select(s => s.Name).ToList(),
                SkillOptions,
                option => OpponentSkill = option,
                SkillOptions.IndexOf(OpponentSkill));
        }
    }

    [Binding]
    public void CreateTest()
    {
        var message = Success ?
            $"Created new success test for {Skill}" :
            $"Created new oppoesed test for {Skill} against {Opponent.Name}'s {OpponentSkill}";

        FeedModel.Instance.AddMessage(
            "GM",
            message,
            new TestData
            {
                PlayerSkill = Skill,
                OpponentSkill = OpponentSkill,
                OpponentName = Opponent.Name,
                OpponentSkillValue = Opponent.Skills.Find(s => s.Name == OpponentSkill).Value,
                OpponentPairedAttributeValue = Opponent.Attributes.Find(a => a.Name == CharacterModel.SkillNamesToRelatedAttrs[OpponentSkill]).Value,
                SkillThreshold = Threshold
            },
            send: true);

        PanelStack.Instance.PopPanel();
        NotificationSystem.DisplayNotification("Test Created");
    }
}
