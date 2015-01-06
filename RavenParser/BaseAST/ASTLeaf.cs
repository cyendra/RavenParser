using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseToken;

namespace RavenParser.BaseAST {
    public class ASTLeaf : ASTree {
        private static List<ASTree> empty = new List<ASTree>();

        protected Token token;
        public Token Token {
            get {
                return token;
            }
        }

        public override string ToString() {
            return token.Text;
        }

        public ASTLeaf(Token t) {
            token = t; 
        }

        #region override

        public override ASTree this[int index] {
            get {
                return null;
            }
        }
        public override int NumChildern {
            get {
                return 0;
            }
        }
        public override string Location() {
            return "at line " + token.LineNumber;
        }
        public override IEnumerator<ASTree> GetEnumerator() {
            return empty.GetEnumerator();
        }

        #endregion
    }
}
