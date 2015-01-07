using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExToken;
namespace RavenParser.ExAST {
    public class IntegerLiteral : ASTLeaf {
        public IntegerLiteral(Token t) : base(t) { }
        public int Value {
            get {
                return Token.IntegerNumber;
            }
        }
    }
}
