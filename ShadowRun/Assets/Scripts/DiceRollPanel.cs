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

    private bool finished;
    [Binding]
    public bool Finished
    {
        get { return finished; }
        set { SetProperty(ref finished, value, nameof(Finished)); }
    }

    private int fivesAndSixes;
    [Binding]
    public int FivesAndSixes
    {
        get { return fivesAndSixes; }
        set { SetProperty(ref fivesAndSixes, value, nameof(FivesAndSixes)); }
    }

    private int ones;
    [Binding]
    public int Ones
    {
        get { return ones; }
        set { SetProperty(ref ones, value, nameof(Ones)); }
    }

    private bool success;
    [Binding]
    public bool Success
    {
        get { return success; }
        set { SetProperty(ref success, value, nameof(Success)); }
    }

    private bool glitch;
    [Binding]
    public bool Glitch
    {
        get { return glitch; }
        set { SetProperty(ref glitch, value, nameof(Glitch)); }
    }

    private bool critGlitch;
    [Binding]
    public bool CritGlitch
    {
        get { return critGlitch; }
        set { SetProperty(ref critGlitch, value, nameof(CritGlitch)); }
    }

    private TestData testData;
    [Binding]
    public TestData TestData
    {
        get { return testData; }
        set { SetProperty(ref testData, value, nameof(TestData)); }
    }

    [SerializeField]
    private DiceRoller diceRoller;

    public override void Init(object args = null)
    {
        if (diceRoller == null)
        {
            Debug.LogError("Dice roller cannot be null");
            return;
        }

        TestData = args as TestData;

        if (TestData == null)
        {
            Debug.LogError("Cannot make a roll without a test");
            BackOut();
            return;
        }

        diceRoller.diceRolls.Clear();
        diceRoller.diceRolls.Add(new DiceRoller.DiceRoll { type = DiceRoller.DiceType.D6, number = 50 });
        diceRoller.SetUpDice(TestData);
    }

    void OnDisable()
    {
        Rolled = false;
        Finished = false;
        FivesAndSixes = 0;
        Ones = 0;
        Glitch = false;
        CritGlitch = false;
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
            return (diceRoller.GetDiceTotal(out _, out _, out _)) != null;
        });
        Finished = true;

        int result = diceRoller.GetDiceTotal(out var fs, out var o, out var numRolled).Value;
        FivesAndSixes = fs;
        Ones = o;
        // Set success based on equation
        Success = FivesAndSixes >= TestData.SkillThreshold;
        Glitch = Ones > numRolled / 2;
        CritGlitch = Glitch && !Success;
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
