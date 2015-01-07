using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class ParameterList : ASTList {
        public ParameterList(List<ASTree> c) : base(c) { }
        public string Name(int i) {
            return ((ASTLeaf)this[i]).Token.Text;
        }
        public int Size {
            get {
                return NumChildern;
            }
        }
    }
}
