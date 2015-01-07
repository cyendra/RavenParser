using RavenParser.BaseException;
using RavenParser.BaseToken;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace RavenParser.BaseLexer {
    public class Lexer : ILexer {
        public static string regexPat = @"\s*(?<id>(?<comments>//.*)|(?<integer>[0-9]+)|(?<string>""(\\""|\\\\|\\n|[^""])*"")|[A-Z_a-z][A-Z_a-z0-9]*|:=|<=|>=|&&|\|\||\p{P}|>|<|=|\+|-|\*|/)?";
        private Regex regex = new Regex(regexPat);
        private Queue<Token> queue = new Queue<Token>();
        private bool hasMore;
        private TextReader reader;
        private int lineNumber;

        public Lexer(TextReader r) {
            hasMore = true;
            reader = r;
            lineNumber = 0;
        }

        #region ILexer 成员

        public Token Read() {
            if (FillQueue(0)) {
                return queue.Dequeue();
            }
            else {
                return Token.EOF;
            }
        }

        public Token Peek(int i) {
            if (FillQueue(i)) {
                return queue.ElementAt(i);
            }
            else {
                return Token.EOF;
            }
        }

        #endregion

        private bool FillQueue(int i) {
            while (i >= queue.Count) {
                if (hasMore) {
                    ReadLine();
                }
                else {
                    return false;
                }
            }
            return true;
        }

        protected void ReadLine() {
            string line;
            try {
                line = reader.ReadLine();
                lineNumber++;
            }
            catch (IOException e) {
                throw new ParseException(e);
            }
            if (line == null) {
                hasMore = false;
                return;
            }
            int pos = 0;
            int endPos = line.Length;
            Match matcher;
            while (pos < endPos) {
                matcher = regex.Match(line, pos);
                if (matcher.Length == 0 || matcher.Index > pos) {
                    throw new ParseException("bad token at line " + lineNumber);
                }
                AddToken(lineNumber, matcher);
                pos += matcher.Length;
            }
            //queue.Enqueue(new IdToken(lineNumber, Token.EOL));
        }
        protected void AddToken(int lineNo, Match matcher) {
            string id = matcher.Groups["id"].Value;
            string comments = matcher.Groups["comments"].Value;
            string integer = matcher.Groups["integer"].Value;
            string str = matcher.Groups["string"].Value;
            if (id != "") {
                if (comments == "") {
                    Token token;
                    if (integer != "") {
                        token = new IntegerToken(lineNo, int.Parse(integer));
                    }
                    else if (str != "") {
                        token = new StrToken(lineNo, ToStringLiteral(str));
                    }
                    else {
                        token = new IdToken(lineNo, id);
                    }
                    queue.Enqueue(token);
                }
            }
        }
        protected string ToStringLiteral(string s) {
            StringBuilder builder = new StringBuilder();
            int len = s.Length - 1;
            for (int i = 1; i < len; i++) {
                char c = s[i];
                if (c == '\\' && i + 1 < len) {
                    int c2 = s[i + 1];
                    if (c2 == '\"' || c2 == '\\') {
                        c = s[++i];
                    }
                    else if (c2 == 'n') {
                        ++i;
                        c = '\n';
                    }
                }
                builder.Append(c);
            }
            return builder.ToString();
        }


    }
}
