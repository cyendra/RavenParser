using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using RavenParser.BaseToken;
using RavenParser.BaseParser;
using RavenParser.BaseLexer;
using RavenParser.BaseAST;
namespace RavenParser.BaseLexer {
    public class Lexer : ILexer {
        public static string regexPat = @"\s*(?<id>(?<comments>//.*)|(?<integer>[0-9]+)|(?<string>""(\\""|\\\\|\\n|[^""])*"")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\|\||\p{P}|>|<|=|\+|-|\*|/)?";
        private Regex regex = new Regex(regexPat);
        private Queue<Token> queue = new Queue<Token>();
        private bool hasMore;
        private TextReader reader;
        private int lineNumber;

        public Lexer(TextReader r) {
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

            Match matcher = regex.Match(line);
      
        }
        protected void AddToken(int lineNo, Match matcher) {

        }
        protected string ToStringLiteral(string s) {
            return null;
        }


    }
}
