using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Base {
    public class BaseDebug {
        public void ShowAll() {

            Symbols symbols = new Symbols();
            Productions productions = new Productions();
            Syntax syntax = new Syntax(symbols, productions);

            symbols.RegistTerm("a");
            symbols.RegistTerm("b");
            symbols.RegistTerm("c");

            symbols.RegistNonterm("S");
            symbols.RegistNonterm("A");
            symbols.RegistNonterm("B");
            symbols.RegistNonterm("C");
            symbols.RegistNonterm("D");

            Production S_AB = new Production("S");
            S_AB.Add("A").Add("B");
            Production S_bC = new Production("S");
            S_bC.Add("b").Add("C");
            Production A_ = new Production("A");
            A_.Add("");
            Production A_b = new Production("A");
            A_b.Add("b");
            Production B_ = new Production("B");
            B_.Add("");
            Production B_aD = new Production("B");
            B_aD.Add("a").Add("D");
            Production C_AD = new Production("C");
            C_AD.Add("A").Add("D");
            Production C_b = new Production("C");
            C_b.Add("b");
            Production D_aS = new Production("D");
            D_aS.Add("a").Add("S");
            Production D_c = new Production("D");
            D_c.Add("c");
            bool rs;
            rs = productions.Products.Add(S_AB);
            rs = productions.Products.Add(S_bC);
            rs = productions.Products.Add(A_);
            rs = productions.Products.Add(A_b);
            rs = productions.Products.Add(B_);
            rs = productions.Products.Add(B_aD);
            rs = productions.Products.Add(C_AD);
            rs = productions.Products.Add(C_b);
            rs = productions.Products.Add(D_aS);
            rs = productions.Products.Add(D_c);

            System.Console.Write(symbols.Show());
            System.Console.Write(productions.Show());

            syntax.CalculateEpsilonSet();
            foreach (var item in syntax.EpsilonSet) {
                System.Console.WriteLine(item.Key + " " + item.Value);
            }
            System.Console.WriteLine();

            foreach (var item in syntax.EpsilonProduction) {
                var key = item.Key;
                var value = item.Value;
                System.Console.WriteLine(key.Show() + " " + value);
            }

            syntax.CalculateFirstSet();
            var firstSet = syntax.FirstSet;
            foreach (var item in firstSet) {
                var key = item.Key;
                var value = item.Value;
                System.Console.Write(key + " : ");
                foreach (var str in value) {
                    System.Console.Write("\"" + str + "\" ");
                }
                System.Console.WriteLine();
            }

            var firstSetOfProduct = syntax.FirstSetOfProduction;
            foreach (var product in firstSetOfProduct) {
                System.Console.WriteLine(product.Key.Show());
                foreach (var str in product.Value) {
                    System.Console.Write("\"" + str + "\" ");
                }
                System.Console.WriteLine();
            }

            syntax.CalculateFollowSet("S");
            var followSet = syntax.FollowSet;
            foreach (var item in followSet) {
                var key = item.Key;
                var value = item.Value;
                System.Console.Write(key + " : ");
                foreach (var str in value) {
                    System.Console.Write("\"" + str + "\" ");
                }
                System.Console.WriteLine();
            }

            syntax.CalculateSelectSet();
            foreach (var item in syntax.SelectSet) {
                var key = item.Key;
                var value = item.Value;
                System.Console.Write(key.Show() + " : ");
                foreach (var str in value) {
                    System.Console.Write("\"" + str + "\" ");
                }
                System.Console.WriteLine();
            }

        }
    }
}
