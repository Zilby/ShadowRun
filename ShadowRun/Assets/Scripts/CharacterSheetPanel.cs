using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;

[Binding]
public class CharacterSheetPanel : Panel
{

    public static Dictionary<string, string> SkillNamesToRelatedAttrs = new Dictionary<string, string>
    {
        { "Ranged W.", "Agility" },
        { "Pistols", "Agility" },
        { "Shotguns", "Agility" },
        { "Rifles", "Agility" },
        { "SMGs", "Agility" },
        { "Dodge", "Agility" },
    };

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
        }

        [Binding]
        public void DecrementValue()
        {
            Data.Value--;
        }

        [Binding]
        public void IncrementBuff()
        {
            Data.Buff++;
        }

        [Binding]
        public void DecrementBuff()
        {
            Data.Buff--;
        }

        [Binding]
        public void Remove()
        {
            removeMe?.Invoke();
        }
    }

    private string _name;
    [Binding]
    public string Name
    {
        get { return _name; }
        private set { SetProperty(ref _name, value, nameof(Name)); }
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

    void Start()
    {
        var savedAttrs = CharacterModel.Instance.Characters.MyCharacter.Attributes;
        var savedSkills = CharacterModel.Instance.Characters.MyCharacter.Skills;
        // If we haven't saved our character yet, create new data;
        foreach (var attr in savedAttrs)
        {
            attributes.Add(new Attribute { Data = attr });
        }

        foreach (var skill in savedSkills)
        {
            var newSkill = new Attribute { Data = skill, Index = SkillOptions.FindIndex(option => option.text == skill.Name) };
            newSkill.removeMe = () => RemoveSkill(newSkill);
            skills.Add(newSkill);

        }
    }

    [Binding]
    public void AddSkill()
    {
        var attr = new Attribute { Data = new AttributeData { Name = "...", Value = 0, Buff = 0 } };
        attr.removeMe = () => RemoveSkill(attr);
        Skills.Add(attr);
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
    }

    private void RemoveSkill(Attribute skill)
    {
        Skills.Remove(skill);
    }
}