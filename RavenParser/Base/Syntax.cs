using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class Syntax {
        private Symbols symbols;
        private Productions productions;

        private Dictionary<string, SortedSet<string>> firstSet;
        private Dictionary<Production, SortedSet<string>> firstSetOfProduction;
        private Dictionary<string, SortedSet<string>> followSet;
        private Dictionary<Production, SortedSet<string>> selectSet;
        private Dictionary<string, bool> epsilonSet;

        public Dictionary<string, bool> EpsilonSet {
            get {
                return epsilonSet;
            }
        }

        public Dictionary<string, SortedSet<string>> FirstSet {
            get {
                return firstSet;
            }
        }

        public Dictionary<Production, SortedSet<string>> FirstSetOfProduction {
            get {
                return firstSetOfProduction;
            }    
        }

        public Syntax(Symbols symbols, Productions productions) {
            this.symbols = symbols;
            this.productions = productions;
            firstSet = new Dictionary<string, SortedSet<string>>();
            firstSetOfProduction = new Dictionary<Production, SortedSet<string>>();
            followSet = new Dictionary<string, SortedSet<string>>();
            selectSet = new Dictionary<Production, SortedSet<string>>();
            epsilonSet = new Dictionary<string, bool>();
        }


        public void CalculateEpsilonSet() {
            epsilonSet.Clear();

            // (1)
            HashSet<Production> del = new HashSet<Production>();
            Dictionary<string, int> cal = new Dictionary<string, int>();
            foreach (var item in productions.Products) {
                if (cal.ContainsKey(item.Name)) {
                    cal[item.Name] += 1;
                }
                else {
                    cal.Add(item.Name, 1);
                }
            }

            string s = symbols.Show();
            string ss = productions.Show();

            // (2)
            foreach (var product in productions.Products) {
                foreach (var item in product.List) {
                    if (symbols.IsTerm(item)) {
                        del.Add(product);
                        cal[product.Name] -= 1;
                        int bk = cal[product.Name];
                        if (cal[product.Name] <= 0) {
                            epsilonSet.Add(product.Name, false);
                        }
                        break;
                    }
                }
            }

            foreach (var product in productions.Products) {
                if (del.Contains(product) == true) continue;
                if (product.List.Count == 1 && product.List[0] == "") {
                    epsilonSet.Add(product.Name, true);
                    foreach (var item in productions.Products) {
                        if (item.Name == product.Name) {
                            del.Add(item);
                            cal[product.Name] -= 1;
                        }
                    }
                }
            }

            // (3)
            bool changed;
            do {
                changed = false;
                foreach (var product in productions.Products) {
                    if (del.Contains(product) == true) continue;
                    bool allEpsilonNonterm = true;
                    foreach (var item in product.List) {
                        if (!symbols.IsNonterm(item) || !(epsilonSet.ContainsKey(item) && epsilonSet[item] == true)) {
                            changed = true;
                            allEpsilonNonterm = false;
                        }
                        if (symbols.IsNonterm(item) && epsilonSet.ContainsKey(item) && epsilonSet[item] == false) {
                            changed = true;
                            del.Add(product);
                            cal[product.Name] -= 1;
                            if (cal[product.Name] <= 0) {
                                epsilonSet.Add(product.Name, false);
                            }
                            break;
                        }
                    }
                    if (allEpsilonNonterm) {
                        epsilonSet.Add(product.Name, true);
                        foreach (var item in productions.Products) {
                            if (del.Contains(product) == true) continue;
                            if (item.Name == product.Name) {
                                del.Add(item);
                                cal[product.Name] -= 1;
                            }
                        }
                    }
                }
            } while (changed);
        }

        public void CalculateFirstSet() {
            SortedSet<string> epsilon = new SortedSet<string>();
            epsilon.Add("");
            firstSet.Clear();

            foreach (var term in symbols.Terms) {
                firstSet.Add(term, new SortedSet<string>());
                firstSet[term].Add(term);
            }
            firstSet.Add("", new SortedSet<string>()); ;
            firstSet[""].Add("");
            foreach (var nonterm in symbols.Nonterms) {
                firstSet.Add(nonterm, new SortedSet<string>());
            }
            foreach (var product in productions.Products) {
                firstSetOfProduction.Add(product, new SortedSet<string>());
            }


            bool changed;
            do {
                changed = false;
                foreach (var nonterm in symbols.Nonterms) {
                    foreach (var product in productions.Products) {
                        if (product.Name != nonterm) continue;
                        if (product.List.Count == 1 && symbols.IsEpsilon(product.List[0])) {
                            if (!firstSet[nonterm].Contains(product.List[0])) {
                                firstSet[nonterm].Add(product.List[0]);
                                changed = true;
                            }
                            continue;
                        }
                        SortedSet<string> set = new SortedSet<string>();
                        set.UnionWith(firstSet[nonterm]);
                        for (int i = 0; i < product.List.Count; i++) {
                            string item = product.List[i];
                            if (i == product.List.Count - 1) {
                                set.UnionWith(firstSet[item]);
                                break;
                            }
                            if (symbols.IsNonterm(item) && epsilonSet.ContainsKey(item) && epsilonSet[item] || symbols.IsEpsilon(item)) {
                                set.UnionWith(firstSet[item].Except<string>(epsilon));
                            }
                            else {
                                set.UnionWith(firstSet[item]);
                                break;
                            }
                        }
                        if (!set.SetEquals(firstSet[nonterm])) {
                            firstSet[nonterm] = set;
                            changed = true;
                        }
                    }
                }
            } while (changed);

            foreach (var product in productions.Products) {
                firstSetOfProduction[product] = firstSetOfList(product, 0);
            }
        }

        private SortedSet<string> firstSetOfList(Production product, int pos) {
            SortedSet<string> epsilon = new SortedSet<string>();
            epsilon.Add("");
            SortedSet<string> set = new SortedSet<string>();
            bool allEpsilon = true;
            for (int i = pos; i < product.List.Count; i++) {
                string item = product.List[i];
                if (firstSet[item].Contains("")) {
                    set.UnionWith(firstSet[item].Except<string>(epsilon));
                }
                else {
                    set.UnionWith(firstSet[item]);
                    allEpsilon = false;
                    break;
                }
            }
            if (allEpsilon) {
                set.UnionWith(epsilon);
            }
            return set;
        }

        public void CalculateFollowSet() {
            followSet.Clear();
            SortedSet<string> epsilon = new SortedSet<string>();
            epsilon.Add("");
            foreach (var nonterm in symbols.Nonterms) {
                followSet.Add(nonterm, new SortedSet<string>());
            }
            bool changed;
            do {
                changed = false;
                foreach (var product in productions.Products) {
                    for (int i = 0; i < product.List.Count; i++) {
                        string item = product.List[i];
                        if (symbols.IsNonterm(item)) {
                            if (i + 1 < product.List.Count) {
                                var set = firstSetOfList(product, i + 1);
                                if (set.IsSubsetOf(followSet[item]) == false) {
                                    changed = true;
                                    followSet[item].UnionWith(set.Except<string>(epsilon));
                                }
                                if (epsilon.IsSubsetOf(set)) {
                                    var newSet = followSet[product.Name];
                                    if (newSet.IsSubsetOf(followSet[item]) == false) {
                                        changed = true;
                                        followSet[item].UnionWith(newSet);
                                    }
                                }
                            }
                            else {
                                var newSet = followSet[product.Name];
                                if (newSet.IsSubsetOf(followSet[item]) == false) {
                                    changed = true;
                                    followSet[item].UnionWith(newSet);
                                }
                            }
                        }
                    }
                }
            } while (changed);

        }
        public void CalculateSelectSet() {

        }


    }
}