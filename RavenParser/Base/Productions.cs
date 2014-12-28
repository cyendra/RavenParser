using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class Productions {
        private SortedSet<Production> productions;
        private static readonly ProductionComparer productionComparer = new ProductionComparer();
        public Productions() {
            productions = new SortedSet<Production>(productionComparer);
        }
        public bool RegistProduction(Production production) {
            if (productions.Contains(production)) return false;
            productions.Add(production);
            return true;
        }
        public SortedSet<Production> Products {
            get {
                return productions;
            }
        }
        public string Show() {
            StringBuilder builder = new StringBuilder();
            foreach (var item in productions) {
                builder.Append(item.Show());
                builder.Append("\n");
            }
            builder.Append("\n");
            return builder.ToString();
        }
    }
}
