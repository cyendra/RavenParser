using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExToken {
    public class IntegerToken : Token {
        private int value;
        public IntegerToken(int line, int v) : base(line) {
            value = v;
        }
        public override bool IsInteger {
            get {
                return true;
            }
        }
        public override string Text {
            get {
                return value.ToString();
            }
        }
        public override int IntegerNumber {
            get {
                return value;
            }
        }
    }
}
