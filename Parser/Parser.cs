using System;
using System.Collections.Generic;
using System.IO;
using Tokenizer;

namespace Parser
{
    public class Parser
    {
        public static int ParseCount = 0;

        private static readonly Dictionary<string, List<Statement>> _cache = new Dictionary<string, List<Statement>>();
        public static List<Statement> Parse(string root, string file)
        {
            ParseCount++;
            List<Statement> result = new List<Statement>();
            string combined = Path.Combine(root, file);

            if (_cache.ContainsKey(combined)) return _cache[combined];

            List<Token> tokens = Tokenizer.Tokenizer.Parse(File.ReadAllText(combined));

            ParserSettings settings = new ParserSettings();
            settings.Root = root;
            settings.File = combined;
            settings.Tokens = tokens;

            foreach (Token token in tokens)
            {
                foreach (KeyValuePair<Type, Func<ParserSettings, Token, Statement>> statement in possibleStatements)
                {
                    Statement stmt = statement.Value(settings, token);
                    if (stmt != null)
                    {
                        result.Add(stmt);
                        break;
                    }
                }
            }
            _cache.Add(combined, result);
            return result;
        }

        public class ParserSettings
        {
            public List<Token> Tokens { get; set; }
            public string Root { get; set; }
            public string File { get; set; }
        }

        private static readonly Dictionary<Type, Func<ParserSettings, Token, Statement>> possibleStatements = new Dictionary<Type, Func<ParserSettings, Token, Statement>>
        {
            { typeof(IncludeStatement), IncludeStatement.Parse },
            { typeof(ClassStatement), ClassStatement.Parse }
        };
    }
}
