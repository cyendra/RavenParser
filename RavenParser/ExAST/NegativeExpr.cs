using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
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
