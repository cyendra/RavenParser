using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.BaseParser {
    public class ASTList : ASTree {
        protected List<ASTree> children;
        public ASTList(List<ASTree> list) {
            children = list;
        }
        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            string sep = "";
            foreach (var t in children) {
                builder.Append(sep);
                sep = " ";
                builder.Append(t.ToString());
            }
            return builder.Append(')').ToString();
        }

        #region override

        public override ASTree this[int index] {
            get {
                return children[index];
            }
        }
        public override int NumChildern {
            get {
                return children.Count;
            }
        }
        public override string Location() {
            foreach (var t in children) {
                string s = t.Location();
                if (s != null) {
                    return s;
                }
            }
            return null;
        }
        public override IEnumerator<ASTree> GetEnumerator() {
            return children.GetEnumerator();
        }

        #endregion

    }
}
