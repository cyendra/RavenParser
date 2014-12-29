using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class Item {
        private Production product;
        private int maxDot;
        private int dot;
        public int MaxDot {
            get {
                return maxDot;
            }
        }
        public int Dot {
            get {
                return dot;
            }
        }
        public string Name {
            get {
                return product.Name;
            }
        }
        public List<string> List {
            get {
                return product.List;
            }
        }
        public Item(Production production) {
            product = production;
            if (product.List.Count == 1 && product.List[0] == "") {
                maxDot = 0;
            }
            else {
                maxDot = product.List.Count; ;
            }
            dot = 0;
        }
        public Item(Production production,int nowDot,int dotLimit) {
            product = production;
            maxDot = dotLimit;
            dot = nowDot;
        }

        public Item Clone(int pos) {
            if (pos > MaxDot) return null;
            Item item = new Item(product, pos, MaxDot);
            return item;
        }

        public List<Item> CloneList() {
            List<Item> list = new List<Item>();
            list.Add(this);
            for (int i = 1; i <= MaxDot; i++) {
                list.Add(Clone(i));
            }
            return list;
        }
        static public List<Item> CloneList(Production production) {
            Item item = new Item(production);
            return item.CloneList();
        }

        public string Show() {
            if (MaxDot == 0) {
                return Name + " : .";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + " : ");
            if (Dot == 0) {
                builder.Append(".");
            }
            for (int i = 0; i < List.Count; i++) {
                builder.Append(List[i]);
                if (Dot == i + 1) {
                    builder.Append(".");
                }
            }
            return builder.ToString();
        }
    }
}
