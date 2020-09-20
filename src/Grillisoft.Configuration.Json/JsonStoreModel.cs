using System.Collections.Generic;
namespace src.Grillisoft.Configuration.Json
{
    public class JsonStoreModel
    {
        public string Parent {get; set;}

        public IDictionary<string, string> Keys {get; set;}
    }
}