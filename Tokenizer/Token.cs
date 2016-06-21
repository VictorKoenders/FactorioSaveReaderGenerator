using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tokenizer
{
    public class Token
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StartingToken { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Content { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Token> Children { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EndingToken { get; set; }
    }
}