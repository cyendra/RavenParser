using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExAST;
using RavenParser.BaseParser;
using RavenParser.ExToken;

namespace RavenParser.ExAST {
    public class Lambda : ASTList {
        public Lambda(List<ASTree> c) : base(c) { }
        public ParameterList Parameters {
            get {
                return this[0] as ParameterList;
            }
        }
        public BlockStmt Body {
            get {
                return this[1] as BlockStmt;
            }
        }
        public override string ToString() {
            return "(lambda " + Parameters + " " + Body + ")";
        }
    }
}
