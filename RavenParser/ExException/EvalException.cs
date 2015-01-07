using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExException {
    public class EvalException : Exception {
        public EvalException(string s) : base(s) { }
        public EvalException(string s, ASTree t) : base(s + " " + t.Location()) { }
    }
}
