using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RavenParser.BaseParser;

namespace RavenParser.ExAST {
    public class Dot : Postfix {
        public Dot(List<ASTree> c) : base(c) { }
        public string Name {
            get {
                return (this[0] as ASTLeaf).Token.Text;
            }
        }
        public override string ToString() {
            return "." + Name;
        }
    }
}
