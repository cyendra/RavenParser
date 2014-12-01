using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms
{
    public class Nonterminal : INonterminal
    {
        public Nonterminal() {
            _list = new List<IEnumerable<string>>();
        }

        private List<IEnumerable<string>> _list;

        #region INonterminal 成员

        public void Add(IEnumerable<string> iter) {
            _list.Add(iter);
        }

        public int Size {
            get {
                return _list.Count;
            }
        }

        public bool Remove(int idx) {
            if (idx >= Size) return false;
            _list.RemoveAt(idx);
            return true;
        }

        public IEnumerable<string> At(int idx) {
            if (idx >= Size) return null;
            return _list.ElementAt(idx);
        }

        #endregion
    }
}
