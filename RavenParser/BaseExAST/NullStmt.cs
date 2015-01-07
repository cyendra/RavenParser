using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;
using RavenParser.BaseAST;
namespace RavenParser.BaseExAST {
    public class NullStmt : ASTList {
        public NullStmt(List<ASTree> c) : base(c) { }
    }
}
