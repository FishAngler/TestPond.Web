namespace TestPond.WebAPI
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize(object toBeSerialized);
    }
}