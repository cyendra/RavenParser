using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RavenParser.Terms;

namespace RavenParser.Lex {
    public class Lexer {
        
        private string _text;
        private int _pos;
        public void SetText(string text) {
            _text = text;
            _pos = 0;
        }

        private Grammars _grammer;
        public void SetGrammer(Grammars gram) {
            _grammer = gram;
        }

        Token Scan() {
            Token tok = null;
            bool ignore = true;
            while (ignore) {
                ignore = false;
                _grammer.MapToIgnores((x) => {
                    if (ignore) return;
                    var mat = x.Regex.Match(_text, _pos);
                    if (mat.Success) {
                        _pos += mat.ToString().Length;
                        ignore = true;
                        return;
                    }
                });
            }
            bool ok = false;
            _grammer.MapToTerm((x) => {
                if (ok) return;
                var mat = x.Regex.Match(_text, _pos);
                if (mat.Success) {
                    tok = new Token(x.Name, mat.ToString());
                    ok = true;
                    return;
                }
            });
            return tok;
        }
    
    }
}
