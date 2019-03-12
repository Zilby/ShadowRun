using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTest : MonoBehaviour
{
    public void MakeTest()
    {
        FeedModel.Instance.AddMessage("Game Master", $"Created new opposed test for {CharacterSheetPanel.SkillOptions[2].text}", true);
    }
}
