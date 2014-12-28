using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RavenParser.Base;
namespace UnitTest {
    [TestClass]
    public class BaseAlgorithmTest {

        [TestMethod]
        public void Test() {
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
            rs = productions.Products.Add(S_AB); Assert.IsTrue(rs);
            rs = productions.Products.Add(S_bC); Assert.IsTrue(rs);
            rs = productions.Products.Add(A_); Assert.IsTrue(rs);
            rs = productions.Products.Add(A_b); Assert.IsTrue(rs);
            rs = productions.Products.Add(B_); Assert.IsTrue(rs);
            rs = productions.Products.Add(B_aD); Assert.IsTrue(rs);
            rs = productions.Products.Add(C_AD); Assert.IsTrue(rs);
            rs = productions.Products.Add(C_b); Assert.IsTrue(rs);
            rs = productions.Products.Add(D_aS); Assert.IsTrue(rs);
            rs = productions.Products.Add(D_c); Assert.IsTrue(rs);

            symbols.Show();
            productions.Show();

            syntax.CalculateEpsilonSet();
            Assert.IsTrue(syntax.EpsilonSet["S"]);
            Assert.IsTrue(syntax.EpsilonSet["A"]);
            Assert.IsTrue(syntax.EpsilonSet["B"]);
            Assert.IsFalse(syntax.EpsilonSet["C"]);
            Assert.IsFalse(syntax.EpsilonSet["D"]);

            syntax.CalculateFirstSet();
            var firstSet = syntax.FirstSet;
            foreach (var item in firstSet) {
                var key = item.Key;
                var value = item.Value;
            }

            Assert.AreEqual(3, firstSet["S"].Count);
            Assert.AreEqual(2, firstSet["A"].Count);
            Assert.AreEqual(2, firstSet["B"].Count);
            Assert.AreEqual(3, firstSet["C"].Count);
            Assert.AreEqual(2, firstSet["D"].Count);
        }
    }
}
