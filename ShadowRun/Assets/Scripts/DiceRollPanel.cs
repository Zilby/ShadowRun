using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class DiceRollPanel : Panel
{
    private bool rolled;
    [Binding]
    public bool Rolled
    {
        get { return rolled; }
        set { SetProperty(ref rolled, value, nameof(Rolled)); }
    }

    private int? result;
    [Binding]
    public int? Result
    {
        get { return result; }
        set { SetProperty(ref result, value, nameof(Result)); }
    }

    [SerializeField]
    private DiceRoller diceRoller;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (diceRoller == null)
        {
            Debug.LogError("Dice roller cannot be null");
            return;
        }
        diceRoller.SetUpDice();
    }

    void OnDisable()
    {
        Rolled = false;
        Result = null;
        diceRoller.ResetRoller();
    }

    private IEnumerator WaitForResult()
    {
        int? ret = null;
        diceRoller.RollDice();
        Rolled = true;
        yield return new WaitUntil(() =>
        {
            return (ret = diceRoller.GetDiceTotal()) != null;
        });
        Result = ret;
    }

    [Binding]
    public void RollDice()
    {
        StartCoroutine(WaitForResult());
    }
}
