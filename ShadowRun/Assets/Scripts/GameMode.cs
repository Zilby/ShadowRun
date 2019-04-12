using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class GameMode : DataBindMonobehaviour
{
    private bool isPlayer = true;
    [Binding]
    public bool IsPlayer
    {
        get { return isPlayer; }
        set { SetProperty(ref isPlayer, value, nameof(IsPlayer)); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
