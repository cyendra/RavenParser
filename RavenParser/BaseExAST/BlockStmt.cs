using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class BlockStmt : ASTList {
        public BlockStmt(List<ASTree> c) : base(c) { }
        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append("(begin");
            char sep = ' ';
            foreach (var t in children) {
                builder.Append(sep);
                builder.Append(t.ToString());
            }
            return builder.Append(')').ToString();
        }
    }
}
