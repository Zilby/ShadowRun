using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

public class CharacterModel
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

    private static CharacterModel instance;
    public static CharacterModel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CharacterModel();
            }
            return instance;
        }
    }

    public CharacterData Characters { get; private set; }

    // Start is called before the first frame update
    private CharacterModel()
    {
        // When creating the character model, we need to load in all the character info
        if (PlayerPrefs.HasKey("CharacterData"))
        {
            var json = PlayerPrefs.GetString("CharacterData");
            Characters = JsonUtility.FromJson<CharacterData>(json);
        }
        else
        {
            // Set up default data if none exists
            Characters = new CharacterData
            {
                MyCharacter = new Character
                {
                    Attributes = new List<AttributeData>
                    {
                        new AttributeData { Name = "Body", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Agility", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Reaction", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Strength", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Willpower", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Logic", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Intuition", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Charisma", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Edge", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Essence", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Initiative", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Magic", Value = 0, Buff = 0 },
                        new AttributeData { Name = "Resonance", Value = 0, Buff = 0 }
                    },
                    Skills = new List<AttributeData>()
                }
            };
        }
    }

    // Update is called once per frame
    public void Save()
    {
        var characterData = JsonUtility.ToJson(Characters, prettyPrint: false);
        PlayerPrefs.SetString("CharacterData", characterData); // Pretending this is an acceptable way to do this...
    }
}

[Serializable]
public class CharacterData
{
    [SerializeField]
    private Character myCharacter;
    public Character MyCharacter
    {
        get { return myCharacter; }
        set { myCharacter = value; }
    }

    [SerializeField]
    private List<Character> npcs;
    public List<Character> NPCs
    {
        get { return npcs; }
        set { npcs = value; }
    }
}

[Serializable]
public class Character
{
    [SerializeField]
    private List<AttributeData> attributes;
    public List<AttributeData> Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

    [SerializeField]
    private List<AttributeData> skills;
    public List<AttributeData> Skills
    {
        get { return skills; }
        set { skills = value; }
    }
}

[Binding]
[Serializable]
public class AttributeData : DataBindObject
{
    [SerializeField]
    private string name;
    [Binding]
    public string Name
    {
        get { return name; }
        set { SetProperty(ref name, value, nameof(Name)); }
    }

    [Binding]
    public int Total { get { return Value + Buff; } }

    [SerializeField]
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

    [SerializeField]
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
}