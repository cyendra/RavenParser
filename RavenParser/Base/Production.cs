using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {

    public class ProductionComparer : Comparer<Production> {
        public override int Compare(Production x, Production y) {
            if (x.Name == y.Name) {
                int p = 0;
                do {
                    if (p == x.List.Count && p == y.List.Count) {
                        return 0;
                    }
                    else if (p == x.List.Count) {
                        return -1;
                    }
                    else if (p == y.List.Count) {
                        return 1;
                    }
                    int rs = x.List[p].CompareTo(y.List[p]);
                    if (rs != 0) return rs;
                    p++;
                } while (true);
            }
            return x.Name.CompareTo(y.Name);
        }
    }

    public class Production {
        private string name;
        private List<string> list;

        public string Name {
            get {
                return name;
            }
        }
        public List<string> List {
            get {
                return list;
            }
        }

        public Production(string name) {
            list = new List<string>();
            this.name = name;
        }

        public Production Add(string symbol) {
            list.Add(symbol);
            return this;
        }

        public void Clear() {
            list.Clear();
        }

        public string Show() {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + " -> ");
            foreach (var item in List) {
                if (item == "") {
                    builder.Append("ε ");
                }
                else {
                    builder.Append(item + " "); 
                }
            }
            return builder.ToString();
        }


    }
}
