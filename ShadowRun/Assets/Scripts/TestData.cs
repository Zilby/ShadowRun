using Newtonsoft.Json;

public class TestData
{
    public string PlayerSkill { get; set; }
    public string OpponentSkill { get; set; }
    public int SkillThreshold { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}