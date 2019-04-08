using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;

[Binding]
public class CharacterSheetPanel : Panel
{

    public static List<Dropdown.OptionData> SkillOptions = new List<Dropdown.OptionData>()
    {
        new Dropdown.OptionData("..."),
        new Dropdown.OptionData("Ranged W."),
        new Dropdown.OptionData("Pistols"),
        new Dropdown.OptionData("Shotguns"),
        new Dropdown.OptionData("Rifles"),
        new Dropdown.OptionData("SMGs"),
        new Dropdown.OptionData("Dodge"),
    };

    [Binding]
    public class Attribute : DataBindObject
    {
        public Action removeMe;
        public Action unsavedChanges;

        [Binding]
        public List<Dropdown.OptionData> Options => CharacterSheetPanel.SkillOptions;

        private AttributeData data;
        [Binding]
        public AttributeData Data
        {
            get { return data; }
            set { SetProperty(ref data, value, nameof(Data)); }
        }

        private int index;
        // We need this to initialize the dropdown to the correct value (sadly).
        [Binding]
        public int Index
        {
            get { return index; }
            set
            {
                if (SetProperty(ref index, value, nameof(Index)))
                {
                    Data.Name = SkillOptions[index].text;
                }
            }
        }

        [Binding]
        public void IncrementValue()
        {
            Data.Value++;
            unsavedChanges?.Invoke();
        }

        [Binding]
        public void DecrementValue()
        {
            Data.Value--;
            unsavedChanges?.Invoke();
        }

        [Binding]
        public void IncrementBuff()
        {
            Data.Buff++;
            unsavedChanges?.Invoke();
        }

        [Binding]
        public void DecrementBuff()
        {
            Data.Buff--;
            unsavedChanges?.Invoke();
        }

        [Binding]
        public void Remove()
        {
            removeMe?.Invoke();
            unsavedChanges?.Invoke();
        }
    }

    private string _name;
    [Binding]
    public string Name
    {
        get { return _name; }
        private set { SetProperty(ref _name, value, nameof(Name)); }
    }

    private bool unsavedChanges;
    [Binding]
    public bool UnsavedChanges
    {
        get { return unsavedChanges; }
        set { SetProperty(ref unsavedChanges, value, nameof(UnsavedChanges)); }
    }

    private ObservableList<Attribute> attributes = new ObservableList<Attribute>();
    [Binding]
    public ObservableList<Attribute> Attributes
    {
        get { return attributes; }
        private set { SetProperty(ref attributes, value, nameof(Attributes)); }
    }

    private ObservableList<Attribute> skills = new ObservableList<Attribute>();
    [Binding]
    public ObservableList<Attribute> Skills
    {
        get { return skills; }
        private set { SetProperty(ref skills, value, nameof(Skills)); }
    }

    void OnEnable()
    {
        var savedAttrs = CharacterModel.Instance.Characters.MyCharacter.Attributes;
        var savedSkills = CharacterModel.Instance.Characters.MyCharacter.Skills;
        attributes.Clear();
        skills.Clear();

        // If we haven't saved our character yet, create new data;
        foreach (var attr in savedAttrs)
        {
            var newAttr = new Attribute { Data = attr };
            newAttr.unsavedChanges += MarkUnsavedChanges;
            attributes.Add(newAttr);
        }

        foreach (var skill in savedSkills)
        {
            var newSkill = new Attribute { Data = skill, Index = SkillOptions.FindIndex(option => option.text == skill.Name) };
            newSkill.removeMe = () => RemoveSkill(newSkill);
            newSkill.unsavedChanges += MarkUnsavedChanges;
            skills.Add(newSkill);
        }
    }

    [Binding]
    public void AddSkill()
    {
        var skill = new Attribute { Data = new AttributeData { Name = "...", Value = 0, Buff = 0 } };
        skill.removeMe = () => RemoveSkill(skill);
        skill.unsavedChanges += MarkUnsavedChanges;
        Skills.Add(skill);
        UnsavedChanges = true;
    }

    [Binding]
    public void SaveChanges()
    {
        var MyCharacter = CharacterModel.Instance.Characters.MyCharacter;
        MyCharacter.Attributes.Clear();
        foreach (var attr in attributes)
        {
            MyCharacter.Attributes.Add(attr.Data);
        }
        MyCharacter.Skills.Clear();
        foreach (var skill in skills)
        {
            MyCharacter.Skills.Add(skill.Data);
        }
        CharacterModel.Instance.Save();
        UnsavedChanges = false;
    }

    private void RemoveSkill(Attribute skill)
    {
        skill.unsavedChanges -= MarkUnsavedChanges;
        Skills.Remove(skill);
        MarkUnsavedChanges();
    }

    private void MarkUnsavedChanges()
    {
        UnsavedChanges = true;
    }
}