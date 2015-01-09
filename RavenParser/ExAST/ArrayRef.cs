using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class ArrayRef : Postfix {
        public ArrayRef(List<ASTree> c) : base(c) { }
        public ASTree Index {
            get {
                return this[0];
            }
        }
        public override string ToString() {
            return "[" + Index + "]";
        }
    }
}
