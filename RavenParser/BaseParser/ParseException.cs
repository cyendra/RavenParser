using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;

namespace RavenParser.BaseParser {
    public class ParseException : Exception {
        public ParseException(Token t) : this("", t) { }
        public ParseException(string msg, Token t) : base("syntax error around " + Location(t) + ". " + msg) { }
        private static string Location(Token t) {
            if (t == Token.EOF) {
                return "the last line";
            }
            else {
                return "\"" + t.Text + "\" at line " + t.LineNumber;
            }
        }
        public ParseException(Exception e) : base("", e) { }
        public ParseException(string msg) : base(msg) { }
    }
}
