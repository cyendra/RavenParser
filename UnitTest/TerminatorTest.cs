using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RavenParser.Terms;
using System.Collections.Generic;
namespace UnitTest {
    [TestClass]
    public class TerminalTest {
        [TestMethod]
        public void TestTerminal() {
            ITerminator term = new Terminal("id", @"[a-zA-Z]+");
            Assert.AreEqual(term.Name, "id");
            Assert.AreEqual(term.Rule, @"[a-zA-Z]+");
            Assert.AreEqual(term.Weight, 0);
        }

        [TestMethod]
        public void TestTerminalManager() {
            TerminalManager manager = new TerminalManager();
            manager.Regist(new Terminal("1", @"[a-zA-Z]+"));
            manager.Regist(new Terminal("2", @"[0-9]+", 5));
            manager.Regist(new Terminal("3", @"such"));
            manager.Regist(new Terminal("4", @"fff"));
            manager.Regist(new Terminal("5", @"qqq", 100));
            string s = "";
            manager.MapToTerm(term => s += term.Name);
            Assert.AreEqual("52134", s);
            Assert.AreEqual("such", manager.GetTermByName("3").Rule);
            Assert.IsTrue(manager.IsRegist("2"));
            Assert.IsFalse(manager.IsRegist("12345"));
            bool rs;
            rs = manager.Contains("1");
            Assert.IsTrue(rs);
            rs = manager.RemoveTermByName("1");
            Assert.IsTrue(rs);
            Assert.IsFalse(manager.Contains("1"));

            rs = manager.Ignore(new Terminal("5", @"asfd"));
            Assert.IsFalse(rs);

            manager.Ignore(new Terminal("1234", @"asfd"));
            Assert.IsTrue(manager.IsIgnore("1234"));
            Assert.IsTrue(manager.Contains("1234"));
            Assert.IsTrue(manager.RemoveTermByName("1234"));
            Assert.IsFalse(manager.Contains("1234"));
            Assert.IsFalse(manager.IsIgnore("1234"));
        }

        [TestClass]
        public class NonterminalTest {
            [TestMethod]
            public void TestNonterminal() {
                Nonterminal term = new Nonterminal();
                string[] arr = { "123", "345" };
                term.Add(arr);
                List<string> list = new List<string>() {"qqq","ddd"};
                term.Add(list);
                Assert.AreEqual(2, term.Size);
                term.Add(new List<string>() { "ggg", "www" });
                Assert.AreEqual(3, term.Size);
            }
        }

    }
}
