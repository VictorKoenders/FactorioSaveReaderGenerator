using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Tokenizer;

namespace Parser
{
    public class IncludeStatement : Statement
    {
        public string File { get; set; }
        public List<Statement> Statements { get; set; }

        private IncludeStatement(Parser.ParserSettings settings, string file)
        {
            File = file;

            char type = file[0];
            file = file.Substring(1, file.Length - 2);

            if (!file.EndsWith(".hpp")) return;
            if (file.StartsWith("boost/")) return;
            if (file.StartsWith("Agui/")) return;

            //Console.WriteLine("Loading {0}", file);
            string root = settings.Root;
            if (type == '"')
            {
                string prefix = Path.GetDirectoryName(settings.File).Substring(settings.Root.Length);
                file = prefix + "/" + file;
            }
            Statements = Parser.Parse(root, file);
        }

        private static readonly Regex regex = new Regex("^#include (<?\"?[^\\r\\n]+?\"?>?)$", RegexOptions.Compiled);

        public static Statement Parse(Parser.ParserSettings settings, Token token)
        {
            if (string.IsNullOrEmpty(token.Content)) return null;
            Match match = regex.Match(token.Content.Trim());
            if (match.Success)
            {
                string file = match.Groups[1].Value;
                return new IncludeStatement(settings, file);
            }
            return null;
        }
    }
}