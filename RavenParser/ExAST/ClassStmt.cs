using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExAST {
    public class ClassStmt : ASTList {
        public ClassStmt(List<ASTree> c) : base(c) { }
        public string Name {
            get {
                return (this[0] as ASTLeaf).Token.Text;
            }
        }
        public string SuperClass {
            get {
                if (NumChildern < 3) { 
                    return null;
                }
                else {
                    return (this[1] as ASTLeaf).Token.Text;
                }
            }
        }
        public ClassBody Body {
            get {
                return this[NumChildern - 1] as ClassBody;
            }
        }
        public override string ToString() {
            string parent = SuperClass;
            if (parent == null) {
                parent = "*";
            }
            return "(class " + Name + " " + parent + " " + Body + ")";
        }
    }
}
