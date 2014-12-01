using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RavenParser.Terms;

namespace UnitTest
{
    [TestClass]
    public class TerminatorTest
    {
        [TestMethod]
        public void TestTerminator()
        {
            ITerminator term = new Terminator("id", @"[a-zA-Z]+");
            Assert.AreEqual(term.Name, "id");
            Assert.AreEqual(term.Rule, @"[a-zA-Z]+");
            Assert.AreEqual(term.Weight, 0);
        }

        [TestMethod]
        public void TestTerminatorManager()
        {
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
        }
    }
}
