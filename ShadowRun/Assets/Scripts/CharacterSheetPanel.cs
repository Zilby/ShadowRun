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
        [Binding]
        public List<Dropdown.OptionData> Options => CharacterSheetPanel.SkillOptions;

        private string name;
        [Binding]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value, nameof(Name)); }
        }

        private int val;
        [Binding]
        public int Value
        {
            get { return val; }
            set
            {
                if (SetProperty(ref val, value, nameof(Value)))
                {
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        private int buff;
        [Binding]
        public int Buff
        {
            get { return buff; }
            set
            {
                if (SetProperty(ref buff, value, nameof(Buff)))
                {
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        [Binding]
        public int Total { get { return Value + Buff; } }

        [Binding]
        public void IncrementValue()
        {
            Value++;
        }

        [Binding]
        public void DecrementValue()
        {
            Value--;
        }

        [Binding]
        public void IncrementBuff()
        {
            Buff++;
        }

        [Binding]
        public void DecrementBuff()
        {
            Buff--;
        }

        [Binding]
        public void UpdateName()
        {
            OnPropertyChanged(nameof(Name));
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
        attributes.Add(new Attribute { Name = "Body", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Agility", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Reaction", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Strength", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Willpower", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Logic", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Intuition", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Charisma", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Edge", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Essence", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Initiative", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Magic", Value = 0, Buff = 0 });
        attributes.Add(new Attribute { Name = "Resonance", Value = 0, Buff = 0 });
    }

    [Binding]
    public void AddSkill()
    {
        Skills.Add(new Attribute { Name = "", Value = 0, Buff = 0 });
    }
}