using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class Symbols {
        #region 私有
        private SortedSet<string> terms;
        private SortedSet<string> nonterms;
        #endregion

        #region 构造
        public Symbols() {
            terms = new SortedSet<string>();
            nonterms = new SortedSet<string>();
        }
        #endregion

        public string Show() {
            StringBuilder builder = new StringBuilder();
            foreach (var item in terms) {
                builder.Append(item + " ");
            }
            builder.Append("\n");
            foreach (var item in nonterms) {
                builder.Append(item + " ");
            }
            builder.Append("\n");
            builder.Append("\n");
            return builder.ToString();
        }

        #region 查找
        public bool IsEpsilon(string name) {
            return name == "";
        }
        public bool IsTerm(string name) {
            return terms.Contains(name);
        }
        public bool IsNonterm(string name) {
            return nonterms.Contains(name);
        }
        public bool IsSymbol(string name) {
            return terms.Contains(name) || nonterms.Contains(name);
        }
        #endregion

        #region 注册
        public bool RegistTerm(string name) {
            if (IsSymbol(name)) return false;
            terms.Add(name);
            return true;
        }
        public bool RegistNonterm(string name) {
            if (IsSymbol(name)) return false;
            nonterms.Add(name);
            return true;
        }
        #endregion

        #region Get
        public SortedSet<string> Terms {
            get {
                return terms;
            }
        }
        public SortedSet<string> Nonterms {
            get {
                return nonterms;
            }
        }

        #endregion

    }
}
