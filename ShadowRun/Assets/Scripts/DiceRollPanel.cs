using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class DiceRollPanel : Panel
{
    [SerializeField]
    private FadeableUI betweenRollsCurtain;

    [Binding]
    public class DiceResult : DataBindObject
    {
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
    }

    private string opponentName;
    [Binding]
    public string OpponentName
    {
        get { return opponentName; }
        set { SetProperty(ref opponentName, value, nameof(OpponentName)); }
    }

    private DiceResult playerResult;
    [Binding]
    public DiceResult PlayerResult
    {
        get { return playerResult; }
        set { SetProperty(ref playerResult, value, nameof(PlayerResult)); }
    }

    private DiceResult opponentResult;
    [Binding]
    public DiceResult OpponentResult
    {
        get { return opponentResult; }
        set { SetProperty(ref opponentResult, value, nameof(OpponentResult)); }
    }

    private bool isSuccessTest;
    [Binding]
    public bool IsSuccessTest
    {
        get { return isSuccessTest; }
        set { SetProperty(ref isSuccessTest, value, nameof(IsSuccessTest)); }
    }

    private int numPlayerDice;
    [Binding]
    public int NumPlayerDice => CalculateMyDice(TestData);

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

    private bool success;
    [Binding]
    public bool Success
    {
        get { return success; }
        set { SetProperty(ref success, value, nameof(Success)); }
    }

    private bool draw;
    [Binding]
    public bool Draw
    {
        get { return draw; }
        set { SetProperty(ref draw, value, nameof(Draw)); }
    }

    private TestData testData;
    [Binding]
    public TestData TestData
    {
        get { return testData; }
        set
        {
            if (SetProperty(ref testData, value, nameof(TestData)))
            {
                OnPropertyChanged(nameof(NumPlayerDice));
            }
        }
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

        IsSuccessTest = string.IsNullOrEmpty(TestData.OpponentSkill);

        OpponentName = TestData.OpponentName;

        Rolled = false;
        Finished = false;
        Success = false;
        PlayerResult = new DiceResult();
        OpponentResult = new DiceResult();
    }

    void OnDisable()
    {
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

    private IEnumerator WaitForResult(DiceResult results, int threshold)
    {
        diceRoller.RollDice();
        yield return new WaitUntil(() =>
        {
            return (diceRoller.GetDiceTotal(out _, out _, out _)) != null;
        });

        int result = diceRoller.GetDiceTotal(out var fs, out var o, out var numRolled).Value;
        results.FivesAndSixes = fs;
        results.Ones = o;
        // Set success based on equation
        results.Success = results.FivesAndSixes >= threshold;
        results.Glitch = results.Ones > numRolled / 2;
        results.CritGlitch = results.Glitch && !results.Success;
    }

    private IEnumerator WaitForResultSuccess()
    {
        diceRoller.SetUpDice(CalculateMyDice(TestData));
        yield return WaitForResult(PlayerResult, TestData.SkillThreshold);
        Finished = true;
        Success = PlayerResult.Success;

        var name = CharacterModel.Instance.Characters.MyCharacter.Name;
        var message = Success ?
            $"Succeeded roll for {TestData.PlayerSkill}." :
            $"Failed roll for {TestData.PlayerSkill}.";

        if (PlayerResult.Glitch)
        {
            message += "\nGlitch.";
        }

        FeedModel.Instance.AddMessage(name, message, send: true);
    }

    private IEnumerator WaitForResultOpposed()
    {
        diceRoller.SetUpDice(CalculateMyDice(TestData));
        yield return WaitForResult(PlayerResult, 0);
        yield return betweenRollsCurtain.FadeIn();
        diceRoller.ResetRoller();

        var numDice = Math.Max(0, (TestData.OpponentSkillValue + TestData.OpponentPairedAttributeValue) - (TestData.OpponentTotalDamage / 3));

        diceRoller.SetUpDice(numDice);
        yield return betweenRollsCurtain.FadeOut();
        RollDice();
        yield return WaitForResult(OpponentResult, PlayerResult.FivesAndSixes);

        var success = PlayerResult.FivesAndSixes > OpponentResult.FivesAndSixes;
        PlayerResult.Success = success;
        OpponentResult.Success = !success;
        Success = PlayerResult.Success;

        Draw = PlayerResult.FivesAndSixes == OpponentResult.FivesAndSixes;

        Finished = true;
        var name = CharacterModel.Instance.Characters.MyCharacter.Name;
        var message = Draw ? $"Draw against {OpponentName}." :
                Success ?
                    $"Succeeded against {OpponentName}." :
                    $"Failed against {OpponentName}.";

        if (PlayerResult.Glitch)
        {
            message += "\nPlayer glitch.";
        }
        if (OpponentResult.Glitch)
        {
            message += "\nEnemy glitch.";
        }

        FeedModel.Instance.AddMessage(name, message, send: true);
    }

    [Binding]
    public void RollDice()
    {
        if (!Rolled)
        {
            Rolled = true;
            if (IsSuccessTest)
            {
                StartCoroutine(WaitForResultSuccess());
            }
            else
            {
                StartCoroutine(WaitForResultOpposed());
            }
        }
    }

    private int CalculateMyDice(TestData data)
    {
        if (TestData == null)
        {
            return 0;
        }
        var myCharacter = CharacterModel.Instance.Characters.MyCharacter;
        var skill = data.PlayerSkill;
        var numDice = 0;
        if (data.PlayerSkill == "Dodge")
        {
            foreach (var a in myCharacter.Attributes)
            {
                if (a.Name == "Reaction" || a.Name == "Intuition")
                {
                    numDice += a.Value;
                }
            }
            return numDice;
        }

        foreach (var s in myCharacter.Skills)
        {
            print(s.Name + ": " + s.Value);
            if (s.Name.Equals(skill))
            {
                numDice = s.Value;
                break;
            }
        }
        foreach (var s in myCharacter.Attributes)
        {
            print(s.Name + ": " + s.Value);
            if (s.Name.Equals(CharacterModel.SkillNamesToRelatedAttrs[skill]))
            {
                numDice += s.Value;
                break;
            }
        }

        numDice = Math.Max(0, numDice - ((myCharacter.Stun + myCharacter.Damage) / 3));

        return numDice;
    }
}
