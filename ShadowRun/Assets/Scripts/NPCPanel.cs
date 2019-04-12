using System;
using System.Collections.Generic;
using System.Linq;
using UnityWeld.Binding;

[Binding]
public class NPCPanel : Panel
{
    [Binding]
    public class NPCView : DataBindObject
    {
        private NPCPanel panel;

        private Character character;

        private string name;
        [Binding]
        public string Name => Character.Name;

        [Binding]
        public Character Character
        {
            get { return character; }
            private set { SetProperty(ref character, value, nameof(Character)); }
        }

        public NPCView(NPCPanel panel, Character character)
        {
            Character = character;
            this.panel = panel;
        }

        [Binding]
        public void Remove()
        {
            panel.Characters.Remove(this);
            panel.UnsavedChanges = true;
        }

        [Binding]
        public void EditNPC()
        {
            PanelStack.Instance.PushPanel<CharacterSheetPanel>(Character);
            panel.UnsavedChanges = true;
        }

        public void PokeName()
        {
            OnPropertyChanged(nameof(Name));
        }
    }

    private bool unsavedChanges;
    [Binding]
    public bool UnsavedChanges
    {
        get { return unsavedChanges; }
        set { SetProperty(ref unsavedChanges, value, nameof(UnsavedChanges)); }
    }

    private ObservableList<NPCView> characters = new ObservableList<NPCView>();
    [Binding]
    public ObservableList<NPCView> Characters
    {
        get { return characters; }
        set { SetProperty(ref characters, value, nameof(Characters)); }
    }

    private void OnEnable()
    {
        foreach (var npc in Characters)
        {
            npc.PokeName();
        }
    }

    public override void Init(object args = null)
    {
        Characters.Clear();
        foreach (var npc in CharacterModel.Instance.Characters.NPCs)
        {
            var view = new NPCView(this, npc);
            Characters.Add(view);
        }
    }

    [Binding]
    public void AddNPC()
    {
        Characters.Add(new NPCView(this, new Character
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
        }));
        UnsavedChanges = true;
    }

    [Binding]
    public void SaveChanges()
    {
        CharacterModel.Instance.Characters.NPCs.Clear();
        CharacterModel.Instance.Characters.NPCs.AddRange(Characters.Select(view => view.Character));
        CharacterModel.Instance.Save();
    }
}