using System;
using System.Collections.Generic;
using System.Linq;
using UnityWeld.Binding;

/// <summary>
/// Represents a "mobile-style" fullscreen dropdown menu
/// </summary>
[Binding]
public class DropdownWindow : DataBindMonobehaviour
{
    [Binding]
    public class DropdownOption : DataBindObject
    {
        [Binding]
        public string Option { get; private set; }
        [Binding]
        public bool Current { get; private set; }


        [Binding]
        public void PickOption()
        {
            Instance.onSelected?.Invoke(Option);
            Instance.gameObject.SetActive(false);
        }

        public DropdownOption(string option, bool current = false)
        {
            Option = option;
            Current = current;
        }
    }

    private static DropdownWindow Instance { get; set; }

    private Action<string> onSelected;

    private ObservableList<DropdownOption> options = new ObservableList<DropdownOption>();
    [Binding]
    public ObservableList<DropdownOption> Options
    {
        get { return options; }
        private set { SetProperty(ref options, value, nameof(Options)); }
    }

    public static void ShowDropdown(List<string> options, Action<string> onSelected, int currentIndex = -1)
    {
        Instance.Options.Clear();
        for (int i = 0; i < options.Count; i++)
        {
            Instance.Options.Add(new DropdownOption(options[i], currentIndex == i));
        }
        Instance.onSelected = onSelected;
        Instance.gameObject.SetActive(true);
    }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}