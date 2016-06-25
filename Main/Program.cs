using System.Collections.Generic;
using Parser;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string fileContents = File.ReadAllText();
            List<Token> tokens = Tokenizer.Tokenizer.Parse(fileContents);
            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(tokens);*/

            List<Statement> statements = Parser.Parser.Parse("D:\\Development\\C++\\Factorio\\src\\", "Entity\\Accumulator.cpp");
        }
    }
}
