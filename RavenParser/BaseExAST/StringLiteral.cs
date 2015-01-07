using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class StringLiteral : ASTLeaf {
        public StringLiteral(Token t) : base(t) { }
        public string Value {
            get {
                return Token.Text;
            }
        }
    }
}
