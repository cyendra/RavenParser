using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class BinaryExpr : ASTList {
        public BinaryExpr(List<ASTree> c) : base(c) { }
        public ASTree Left {
            get {
                return this[0];
            }
        }
        public string Operator {
            get {
                ASTLeaf op = this[1] as ASTLeaf;
                return op.Token.Text;
            }
        }
        public ASTree Right {
            get {
                return this[2];
            }
        }
    }
}
