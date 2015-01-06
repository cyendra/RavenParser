using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace RavenParser.Base {
    public class BaseDebug {

        public void RegexTest() {
            string num = @"[0-9]+";
            string id = @"[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\|\||\p{P}|>|<|=|\+|-|\*|/";
            string sp = @"""(\\""|\\\\|\\n|[^""])*""";
            string reg = @"\s*(?<id>(?<comments>//.*)|(?<integer>" + num + ")|(?<string>" + sp + ")|" + id + ")?";
            string final = @"\s*(?<id>(?<comments>//.*)|(?<integer>[0-9]+)|(?<string>""(\\""|\\\\|\\n|[^""])*"")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\|\||\p{P}|>|<|=|\+|-|\*|/)?";

            System.Console.WriteLine(reg==final);
            Regex regex = new Regex(reg);
            string text = @"while i < 10{
    sum = sum + i
    i = i + 1
}
sum";
            string text2 = @" 123456 ""abcde"" abcd > >= < //aaa ";
            Match mat;
            int pos = 0;
            int len = text2.Length;
            while (pos < len) {
                System.Console.WriteLine("pos={0}, len={1}", pos, len);
                mat = regex.Match(text2, pos);
                Console.WriteLine("   Group id: '{0}'", mat.Groups["id"].Value);
                Console.WriteLine("   Group comments: '{0}'", mat.Groups["comments"].Value);
                Console.WriteLine("   Group integer: '{0}'", mat.Groups["integer"].Value);
                Console.WriteLine("   Group string: '{0}'", mat.Groups["string"].Value);
                int groupCtr = 0;
                foreach (Group group in mat.Groups) {
                    groupCtr++;
                    Console.WriteLine("   Group {0}: '{1}'", groupCtr, group.Value);
                    int captureCtr = 0;
                    foreach (Capture capture in group.Captures) {
                        captureCtr++;
                        Console.WriteLine("      Capture {0}: '{1}'", captureCtr, capture.Value);
                    }
                }
                pos += mat.Length;
                System.Console.ReadLine();
            }

            

        }

        public void ShowAll() {

            Symbols symbols = new Symbols();
            Productions productions = new Productions();
            Syntax syntax = new Syntax(symbols, productions);
/*
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
*/
/*
            symbols.RegistTerm("a");
            symbols.RegistTerm("b");
            symbols.RegistTerm("c");

            symbols.RegistNonterm("S");
            symbols.RegistNonterm("X");
            symbols.RegistNonterm("Y");

            Production S_XYa = new Production("S");
            S_XYa.Add("X").Add("Y").Add("a");
            Production X_a = new Production("X");
            X_a.Add("a");
            Production X_Yb = new Production("X");
            X_Yb.Add("Y").Add("b");
            Production Y_c = new Production("Y");
            Y_c.Add("c");
            Production Y_ = new Production("Y");
            Y_.Add("");
            productions.Products.Add(S_XYa);
            productions.Products.Add(X_a);
            productions.Products.Add(X_Yb);
            productions.Products.Add(Y_c);
            productions.Products.Add(Y_);
            */


            System.Console.WriteLine("Symbols:");
            System.Console.Write(symbols.Show());
            System.Console.Write(productions.Show());

            System.Console.WriteLine("Epsilon:");
            syntax.CalculateEpsilonSet();
            foreach (var item in syntax.EpsilonSet) {
                System.Console.WriteLine(item.Key + " " + item.Value);
            }
            System.Console.WriteLine();

            System.Console.WriteLine("Epsilon Production:");
            foreach (var item in syntax.EpsilonProduction) {
                var key = item.Key;
                var value = item.Value;
                System.Console.WriteLine(key.Show() + " " + value);
            }

            System.Console.WriteLine("First:");
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

            System.Console.WriteLine("First Product:");
            var firstSetOfProduct = syntax.FirstSetOfProduction;
            foreach (var product in firstSetOfProduct) {
                System.Console.WriteLine(product.Key.Show());
                foreach (var str in product.Value) {
                    System.Console.Write("\"" + str + "\" ");
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("Follow:");
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

            System.Console.WriteLine("Select:");
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

            System.Console.WriteLine("Grammer:");
            AugmentedGrammar grammar = new AugmentedGrammar(productions, "S");
            System.Console.WriteLine(grammar.Show());

        }
    }
}
