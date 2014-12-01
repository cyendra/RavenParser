using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Lex {
    public class Token {
        public Token(string type, string text) {
            Type = type;
            Text = text;
        }

        private string _type;
        private string _text;

        public string Type {
            get {
                return _type;
            }
            set {
                _type = value;
            }
        }

        public string Text {
            get {
                return _text;
            }
            set {
                _text = value;
            }
        }

    }
}
