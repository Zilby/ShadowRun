using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddFeedMessage : MonoBehaviour
{

    private int count = 1;

    [SerializeField]
    FeedPanel feedPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FeedModel.Instance.AddMessage("GM", $"Hello World {count++}");
        }
    }
}
