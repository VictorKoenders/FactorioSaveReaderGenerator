using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tokenizer
{
    public static class Tokenizer
    {
        private static List<Token> Parse(string data, ref int offset)
        {
            List<Token> result = new List<Token>();

            Dictionary<string, string> ignoredBlockEnds = new Dictionary<string, string>
            {
                { "'", "'" },
                { "\"", "\"" },
                { "/*", "*/" },
            };
            string buffer = "";
            string blockToken = "";
            bool isHashLine = false;

            Action addAndFlushBuffer = () =>
            {
                buffer = buffer.Trim();
                if (!string.IsNullOrEmpty(buffer))
                {
                    result.Add(new Token
                    {
                        Content = buffer
                    });
                    buffer = "";
                }
            };
            Action<char> addAndFlushBufferWithToken = c =>
            {
                buffer = buffer.Trim();
                if (!string.IsNullOrEmpty(buffer))
                {
                    result.Add(new Token
                    {
                        Content = buffer,
                        EndingToken = c.ToString()
                    });
                    buffer = "";
                }
            };

            Dictionary<char, char> pairs = new Dictionary<char, char>();
            pairs.Add('{', '}');
            pairs.Add('(', ')');
            pairs.Add('[', ']');

            for (; offset < data.Length; offset++)
            {
                char c = data[offset];
                if (c == '/' && offset < data.Length - 1 && data[offset + 1] == '/')
                {
                    while (data[offset] != '\n' && offset < data.Length) offset++;
                    offset --;
                    continue;
                }

                foreach (KeyValuePair<string, string> block in ignoredBlockEnds)
                {
                    if (blockToken == "" && data.Length > offset + block.Key.Length && data.Substring(offset, block.Key.Length) == block.Key)
                    {
                        blockToken = block.Key;
                    }
                    if (blockToken == block.Key && data.Length > offset + block.Value.Length && data.Substring(offset, block.Value.Length) == block.Value)
                    {
                        blockToken = "";
                    }
                }
                if (blockToken != "")
                {
                    buffer += c;
                    continue;
                }

                if (c == '#')
                {
                    bool isFirstCharOnLine = true;
                    for (int i = offset - 1; i >= 0; i--)
                    {
                        if (data[i] != '\t' && data[i] != ' ') continue;
                        if (data[i] == '\n')
                        {
                            isFirstCharOnLine = false;
                        }
                        break;
                    }
                    if(isFirstCharOnLine)
                        isHashLine = true;
                }

                if (isHashLine && c == '\n')
                {
                    addAndFlushBuffer();
                    continue;
                }

                if (pairs.ContainsKey(c))
                {
                    addAndFlushBuffer();
                    Token nestedToken = new Token();
                    nestedToken.StartingToken = c.ToString();
                    offset++;
                    nestedToken.Children = Parse(data, ref offset);
                    if (nestedToken.Children.Count == 0) nestedToken.Children = null;

                    Debug.Assert(data[offset] == pairs[c]);
                    nestedToken.EndingToken = data[offset].ToString();
                    result.Add(nestedToken);
                    continue;
                }
                if (pairs.ContainsValue(c))
                {
                    break;
                }
                if (c == ';' || c == ',')
                {
                    addAndFlushBufferWithToken(c);
                }
                else
                {
                    buffer += c;
                }

            }
            addAndFlushBuffer();
            return result;
        }
        public static List<Token> Parse(string data)
        {
            int i = 0;
            return Parse(data, ref i);
        }
    }
}