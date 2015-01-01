using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class ItemCollection {
        private SortedSet<Item> itemSet;
        public SortedSet<Item> ItemSet {
            get {
                return itemSet;
            }
        }
        public ItemCollection() {
            itemSet = new SortedSet<Item>();
        }
        public void Add(Item item) {
            itemSet.Add(item);
        }
        public bool UnionWith(ItemCollection itemCollection) {
            return true;
        }
    }
}
