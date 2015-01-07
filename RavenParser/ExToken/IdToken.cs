using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExToken {
    public class IdToken : Token {
        private string text;
        public IdToken(int line, string id) : base(line) {
            text = id;
        }
        public override bool IsIdentifier {
            get {
                return true;
            }
        }
        public override string Text {
            get {
                return text;
            }
        }
    }
}
