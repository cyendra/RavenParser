using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class WhileStmt : ASTList {
        public WhileStmt(List<ASTree> c) : base(c) { }
        public ASTree Condition {
            get {
                return this[0];
            }
        }
        public ASTree Body {
            get {
                return this[1];
            }
        }
        public override string ToString() {
            return "(while " + Condition + " " + Body + ")";
        }
    }
}
