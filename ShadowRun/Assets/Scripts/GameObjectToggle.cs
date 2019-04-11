using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toggleTrue;

    [SerializeField]
    private GameObject[] toggleFalse;

    [SerializeField]
    private bool toggle;
    public bool Toggle
    {
        get { return toggle; }
        set
        {
            toggle = value;
            foreach (var item in toggleTrue)
            {
                item.SetActive(toggle);
            }
            foreach (var item in toggleFalse)
            {
                item.SetActive(!toggle);
            }
        }
    }


}
