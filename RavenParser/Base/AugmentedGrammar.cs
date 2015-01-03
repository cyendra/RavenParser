using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class AugmentedGrammar {
        private List<Item> items;

        public AugmentedGrammar(Productions productions, string startSymbol) {
            items = new List<Item>();
            Production prod = new Production("[S']");
            prod.Add(startSymbol);
            items.AddRange(Item.CloneList(prod));
            foreach (var product in productions.Products) {
                items.AddRange(Item.CloneList(product));
            }
        }

        public string Show() {
            StringBuilder builder = new StringBuilder();
            foreach (var item in items) {
                builder.Append(item.Show() + "\n");
            }
            return builder.ToString();
        }

        private ItemCollection Closure(Item I) {
            return null;
        }

    }
}
