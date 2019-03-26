using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTest : MonoBehaviour
{
    public void MakeTest()
    {
        FeedModel.Instance.AddMessage("Game Master", $"Created new opposed test against Troll using the {CharacterSheetPanel.SkillOptions[2].text} skill", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NotificationSystem.DisplayNotification("Notify");
        }
    }
}
