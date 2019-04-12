using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld;
using UnityWeld.Binding;


[Binding]
public class CharacterSheetPanel : Panel
{
    private Character originalCharacterRef;

    public static List<string> SkillOptions = CharacterModel.SkillNamesToRelatedAttrs.Keys.ToList();

    [Binding]
    public class Attribute : DataBindObject
    {
        private CharacterSheetPanel panel;

        [Binding]
        public List<string> Options => CharacterSheetPanel.SkillOptions;

        private AttributeData data;
        [Binding]
        public AttributeData Data
        {
            get { return data; }
            set { SetProperty(ref data, value, nameof(Data)); }
        }

        public Attribute(CharacterSheetPanel panel, AttributeData data)
        {
            this.panel = panel;
            this.Data = data;
        }

        [Binding]
        public void ShowOptions()
        {
            var actualOptions = Options.Where(option =>
            {
                return option == Data.Name || !panel.Skills.ToList().Exists(o => o.Data.Name == option);
            })
            .ToList();

            DropdownWindow.ShowDropdown(actualOptions,
                option => Data.Name = option,
                actualOptions.IndexOf(Data.Name));
        }

        [Binding]
        public void IncrementValue()
        {
            Data.Value++;
            panel.MarkUnsavedChanges();
        }

        [Binding]
        public void DecrementValue()
        {
            Data.Value--;
            panel.MarkUnsavedChanges();
        }

        [Binding]
        public void IncrementBuff()
        {
            Data.Buff++;
            panel.MarkUnsavedChanges();
        }

        [Binding]
        public void DecrementBuff()
        {
            Data.Buff--;
            panel.MarkUnsavedChanges();
        }

        [Binding]
        public void Remove()
        {
            panel.Skills.Remove(this);
            panel.MarkUnsavedChanges();
        }
    }

    private string _name;
    [Binding]
    public string Name
    {
        get { return _name; }
        private set
        {
            if (SetProperty(ref _name, value, nameof(Name)))
            {
                UnsavedChanges = true;
            }
        }
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

    public override void Init(object args = null)
    {
        var c = args as Character;
        if (c == null)
        {
            originalCharacterRef = CharacterModel.Instance.Characters.MyCharacter;
        }
        else
        {
            originalCharacterRef = c;
        }

        Name = originalCharacterRef.Name;
        var savedAttrs = originalCharacterRef.Attributes;
        var savedSkills = originalCharacterRef.Skills;
        attributes.Clear();
        skills.Clear();

        // If we haven't saved our character yet, create new data;
        foreach (var attr in savedAttrs)
        {
            var newAttr = new Attribute(this, attr);
            attributes.Add(newAttr);
        }

        foreach (var skill in savedSkills)
        {
            var newSkill = new Attribute(this, skill);
            skills.Add(newSkill);
        }
    }

    [Binding]
    public void AddSkill()
    {
        var skill = new Attribute(this, new AttributeData { Name = "...", Value = 0, Buff = 0 });
        Skills.Add(skill);
        UnsavedChanges = true;
    }

    [Binding]
    public void SaveChanges()
    {
        originalCharacterRef.Attributes.Clear();
        foreach (var attr in attributes)
        {
            originalCharacterRef.Attributes.Add(attr.Data);
        }
        originalCharacterRef.Skills.Clear();
        foreach (var skill in skills)
        {
            originalCharacterRef.Skills.Add(skill.Data);
        }
        originalCharacterRef.Name = Name;

        if (CharacterModel.Instance.Characters.MyCharacter != originalCharacterRef &&
            !CharacterModel.Instance.Characters.NPCs.Contains(originalCharacterRef))
        {
            CharacterModel.Instance.Characters.NPCs.Add(originalCharacterRef);
        }

        CharacterModel.Instance.Save();
        UnsavedChanges = false;
    }

    private void MarkUnsavedChanges()
    {
        UnsavedChanges = true;
    }
}