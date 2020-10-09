using System;
using Newtonsoft.Json;

namespace TestPond.WebAPI
{
    public class JsonService : IJsonService
    {
        public JsonService()
        {

        }

        public string Serialize(object toBeSerialized)
        {
            string result = JsonConvert.SerializeObject(toBeSerialized);
            return result;
        }

        public T Deserialize<T>(string json)
        {
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}
