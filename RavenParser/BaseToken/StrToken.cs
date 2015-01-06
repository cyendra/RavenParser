using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.BaseToken {
    public class StrToken : Token {
        private string literal;
        public StrToken(int line, string str) : base(line) {
            literal = str;
        }
        public override bool IsString {
            get {
                return true;
            }
        }
        public override string Text {
            get {
                return literal;
            }
        }
    }
}
