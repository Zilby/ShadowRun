﻿using System.Collections;
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

    private bool finished;
    [Binding]
    public bool Finished
    {
        get { return finished; }
        set { SetProperty(ref finished, value, nameof(Finished)); }
    }

    private int result;
    [Binding]
    public int Result
    {
        get { return result; }
        set { SetProperty(ref result, value, nameof(Result)); }
    }

    private bool success;
    [Binding]
    public bool Success
    {
        get { return success; }
        set { SetProperty(ref success, value, nameof(Success)); }
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
        diceRoller.diceRolls.Clear();
        diceRoller.diceRolls.Add(new DiceRoller.DiceRoll { type = DiceRoller.DiceType.D6, number = 50 });
        diceRoller.SetUpDice();
    }

    void OnDisable()
    {
        Rolled = false;
        Finished = false;
        Result = 0;
        diceRoller.ResetRoller();
    }

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    float lowPassFilterFactor;
    Vector3 lowPassValue;

    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            RollDice();
        }
    }

    private IEnumerator WaitForResult()
    {
        diceRoller.RollDice();
        Rolled = true;
        yield return new WaitUntil(() =>
        {
            return (diceRoller.GetDiceTotal()) != null;
        });
        Finished = true;
        Result = diceRoller.GetDiceTotal().Value;
        // Set success based on equation
        Success = true;
    }

    [Binding]
    public void RollDice()
    {
        if (!Rolled)
        {
            StartCoroutine(WaitForResult());
        }
    }
}
