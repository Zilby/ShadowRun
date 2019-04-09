
using Newtonsoft.Json;

public class ExtraData
{
    public string Sender { get; set; }
    public TestData TestData { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static ExtraData FromString(string from)
    {
        return JsonConvert.DeserializeObject<ExtraData>(from);
    }
}