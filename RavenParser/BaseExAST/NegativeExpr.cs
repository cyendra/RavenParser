using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class NegativeExpr : ASTList {
        public NegativeExpr(List<ASTree> c) : base(c) { }
        public ASTree Operand {
            get {
                return this[0];
            }
        }
        public override string ToString() {
            return "-" + Operand;
        }
    }
}
