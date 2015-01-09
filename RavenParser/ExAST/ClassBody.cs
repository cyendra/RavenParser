using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class ClassBody : ASTList {
        public ClassBody(List<ASTree> c) : base(c) { }
    }
}
