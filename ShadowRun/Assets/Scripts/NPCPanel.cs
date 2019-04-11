using System;
using UnityWeld.Binding;

[Binding]
public class NPCPanel : Panel
{
    [Binding]
    public class NPCView : DataBindObject
    {
        public Action removeMe;

        private Character character;

        private string name;
        [Binding]
        public string Name
        {
            get { return name; }
            set
            {
                if (SetProperty(ref name, value, nameof(Name)))
                {
                    character.Name = name;
                }
            }
        }

        [Binding]
        public Character Character
        {
            get { return character; }
            set { SetProperty(ref character, value, nameof(Character)); }
        }

        [Binding]
        public void Remove()
        {
            removeMe?.Invoke();
        }

        [Binding]
        public void EditNPC()
        {
            PanelStack.Instance.PushPanel<CharacterSheetPanel>(Character);
        }
    }

    private ObservableList<NPCView> characters;
    [Binding]
    public ObservableList<NPCView> Characters
    {
        get { return characters; }
        set { SetProperty(ref characters, value, nameof(Characters)); }
    }

    public override void Init(object args = null)
    {
        foreach (var npc in CharacterModel.Instance.Characters.NPCs)
        {

        }
    }
}