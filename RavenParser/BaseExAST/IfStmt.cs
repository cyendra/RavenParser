using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class IfStmt : ASTList {
        public IfStmt(List<ASTree> c) : base(c) { }
        public ASTree Condition {
            get {
                return this[0];
            }
        }
        public ASTree ThenBlock {
            get {
                return this[1];
            }
        }
        public ASTree ElseBlock {
            get {
                return NumChildern > 2 ? this[2] : null;
            }
        }
        public override string ToString() {
            return "(if " + Condition + " " + ThenBlock + " " + ElseBlock + ")";
        }
    }
}
