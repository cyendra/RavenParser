using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class AugmentedGrammar {
        private SortedSet<Item> items;

        public AugmentedGrammar(Productions productions, string startSymbol) {
            items = new SortedSet<Item>();
            foreach (var product in productions.Products) {
                items.UnionWith(Item.CloneList(product));
            }
            Production prod = new Production("[S']");
            prod.Add(startSymbol);
            items.UnionWith(Item.CloneList(prod));
        }

        public string Show() {
            StringBuilder builder = new StringBuilder();
            foreach (var item in items) {
                builder.Append(item.Show() + "\n");
            }
            return builder.ToString();
        }
    }
}
