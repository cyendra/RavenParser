using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class NullStmt : ASTList {
        public NullStmt(List<ASTree> c) : base(c) { }
    }
}
