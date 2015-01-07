using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExToken;
namespace RavenParser.ExAST {
    public class Name : ASTLeaf {
        public Name(Token t) : base(t) { }
        public string Text {
            get {
                return Token.Text;
            }
        }
    }
}
