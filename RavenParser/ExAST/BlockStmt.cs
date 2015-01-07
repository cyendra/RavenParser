using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
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
