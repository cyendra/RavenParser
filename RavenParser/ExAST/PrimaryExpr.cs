using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class PrimaryExpr : ASTList {
        public PrimaryExpr(List<ASTree> c) : base(c) { }
        public static ASTree Create(List<ASTree> c) {
            return c.Count == 1 ? c[0] : new PrimaryExpr(c);
        }
        public ASTree Operand {
            get {
                return this[0];
            }
        }
        public Postfix Postfix(int nest) {
            return (Postfix)this[NumChildern - nest - 1];
        }
        public bool HasPostfix(int nest) {
            return NumChildern - nest - 1 > 0;
        }
    }
}
