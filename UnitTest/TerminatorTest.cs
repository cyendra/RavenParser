using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RavenParser.Terms;

namespace UnitTest {
    [TestClass]
    public class TerminatorTest {
        [TestMethod]
        public void TestTerminator() {
            ITerminator term = new Terminator("id", @"[a-zA-Z]+");
            Assert.AreEqual(term.Name, "id");
            Assert.AreEqual(term.Rule, @"[a-zA-Z]+");
            Assert.AreEqual(term.Weight, 0);
        }

        [TestMethod]
        public void TestTerminatorManager() {
            TerminatorManager manager = new TerminatorManager();
            manager.Regist(new Terminator("1", @"[a-zA-Z]+"));
            manager.Regist(new Terminator("2", @"[0-9]+", 5));
            manager.Regist(new Terminator("3", @"such"));
            manager.Regist(new Terminator("4", @"fff"));
            manager.Regist(new Terminator("5", @"qqq", 100));
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

            rs = manager.Ignore(new Terminator("5", @"asfd"));
            Assert.IsFalse(rs);

            manager.Ignore(new Terminator("1234", @"asfd"));
            Assert.IsTrue(manager.IsIgnore("1234"));
            Assert.IsTrue(manager.Contains("1234"));
            Assert.IsTrue(manager.RemoveTermByName("1234"));
            Assert.IsFalse(manager.Contains("1234"));
            Assert.IsFalse(manager.IsIgnore("1234"));
        }
    }
}
