using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Rolls dice. 
/// </summary>
public class DiceRoller : MonoBehaviour
{
    public enum DiceType
    {
        D4 = 0,
        D6 = 1,
        D8 = 2,
        D10 = 3,
        D12 = 4,
        D20 = 5,
    }

    [Serializable]
    public struct DiceRoll
    {
        public DiceType type;
        public int number;
    }

    /// <summary>
    /// Indexed dice by dice type. 
    /// </summary>
    public List<GameObject> dice;

    /// <summary>
    /// List of all the dice and how many of each to roll. 
    /// </summary>
    public List<DiceRoll> diceRolls;

    private List<Dice> diceToBeRolled = new List<Dice>();

    private const float DICE_SPACING = 0.045F;

    private const int X_DICE = 5;
    private const int Y_DICE = 5;
    private const int Z_DICE = 5;

    private float RandomizedSpacing
    {
        get { return DICE_SPACING + UnityEngine.Random.Range(-0.002f, 0.002f); }
    }

    /// <summary>
    /// Sets up all the currently stored dice. 
    /// </summary>
    public void SetUpDice(TestData data)
    {
        diceRolls = new List<DiceRoll>();

        string skill = data.PlayerSkill;
        print(skill);
        int skillval = 0;
        foreach (AttributeData s in CharacterModel.Instance.Characters.MyCharacter.Skills)
        {
            print(s.Name + ": " + s.Value);
            if (s.Name.Equals(skill))
            {
                skillval = s.Value;
                break;
            }
        }
        print("skillval = " + skillval);
        foreach (AttributeData s in CharacterModel.Instance.Characters.MyCharacter.Attributes)
        {
            print(s.Name + ": " + s.Value);
            if (s.Name.Equals(CharacterModel.SkillNamesToRelatedAttrs[skill]))
            {
                skillval += s.Value;
                break;
            }
        }
        print("skillval = " + skillval);
        DiceRoll d6 = new DiceRoll();
        d6.type = DiceType.D6;
        d6.number = skillval;
        diceRolls.Add(d6);

        foreach (DiceRoll d in diceRolls)
        {
            for (int i = 0; i < d.number; ++i)
            {
                Dice die = Instantiate(dice[(int)d.type], transform).GetComponent<Dice>();
                die.transform.localPosition = Vector3.zero;
                die.transform.rotation = UnityEngine.Random.rotation;
                die.Rbody.useGravity = false;
                diceToBeRolled.Add(die);
            }
        }
        // Randomize list
        diceToBeRolled.Shuffle();
        for (int i = 0; i < diceToBeRolled.Count; ++i)
        {
            float xPos = ((i % X_DICE) - 2) * RandomizedSpacing;
            if (diceToBeRolled.Count - i < X_DICE && diceToBeRolled.Count / X_DICE == i / X_DICE)
            {
                xPos += (DICE_SPACING / 2f) * (X_DICE - (diceToBeRolled.Count % X_DICE));
            }
            float yPos = ((i / X_DICE) % Y_DICE) * RandomizedSpacing;
            float zPos = -((i / X_DICE) / Z_DICE) * RandomizedSpacing;
            diceToBeRolled[i].transform.localPosition += new Vector3(xPos, yPos, zPos);
        }
    }

    /// <summary>
    /// Rolls the dice once they're set up. 
    /// </summary>
    public void RollDice()
    {
        foreach (Dice d in diceToBeRolled)
        {
            d.Rbody.useGravity = true;
            d.Rbody.AddForce(new Vector3(0f, 0f, 150f * (1 + (-d.transform.localPosition.z / (RandomizedSpacing * 2f)))));
            d.Rbody.AddTorque(new Vector3(UnityEngine.Random.Range(-20, 20),
                                          UnityEngine.Random.Range(-20, 20),
                                          UnityEngine.Random.Range(-20, 20)));
            d.StartCoroutine(d.Toss());
        }
    }

    /// <summary>
    /// Gets the current total of all the dice. 
    /// </summary>
    /// <returns></returns>
    public int? GetDiceTotal(out int fivesAndSixes, out int ones, out int numDice)
    {
        int? total = 0;
        fivesAndSixes = 0;
        ones = 0;
        numDice = diceToBeRolled.Count;
        foreach (Dice d in diceToBeRolled)
        {
            if (d.Value == null)
            {
                return null;
            }
            if (d.Value >= 5)
            {
                fivesAndSixes++;
            }
            if (d.Value <= 1)
            {
                ones++;
            }
            total += d.Value;
        }
        return total;
    }

    /// <summary>
    /// Resets the dice roller for future use. 
    /// </summary>
    public void ResetRoller()
    {
        //diceRolls = new List<DiceRoll>();
        foreach (Dice d in diceToBeRolled)
        {
            if (d != null)
            {
                Destroy(d.gameObject);
            }
        }
        diceToBeRolled = new List<Dice>();
    }
}
