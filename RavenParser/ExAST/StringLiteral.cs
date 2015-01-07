using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class StringLiteral : ASTLeaf {
        public StringLiteral(Token t) : base(t) { }
        public string Value {
            get {
                return Token.Text;
            }
        }
    }
}
