using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;

namespace RavenParser.ExAST {
    public class DefStmt : ASTList {
        public DefStmt(List<ASTree> c) : base(c) { }
        public string Name {
            get {
                return ((ASTLeaf)this[0]).Token.Text;
            }
        }
        public ParameterList Parameters {
            get {
                return (ParameterList)this[1];
            }
        }
        public BlockStmt Body {
            get {
                return (BlockStmt)this[2];
            }
        }
        public override string ToString() {
            return "(define (" + Name + " " + Parameters + ") " + Body + ")";
        }
    }
}
