using System.IO;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileContents = File.ReadAllText("D:\\Development\\C++\\Factorio\\src\\Entity\\Accumulator.cpp");
            var tokens = Tokenizer.Tokenizer.Parse(fileContents);
            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(tokens);
        }
    }
}
