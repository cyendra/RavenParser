using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
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
        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append('(')
                .Append(Operator).Append(" ")
                .Append(Left.ToString()).Append(" ")
                .Append(Right.ToString())
                .Append(')');
            return builder.ToString();
        }
    }
}
