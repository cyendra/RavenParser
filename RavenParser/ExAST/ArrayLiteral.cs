using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class ArrayLiteral : ASTList {
        public ArrayLiteral(List<ASTree> c) : base(c) { }
        public int Size {
            get {
                return NumChildern;
            }
        }
    }
}
