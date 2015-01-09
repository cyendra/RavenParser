using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using RavenParser.ExException;
using RavenParser.BaseParser;
namespace RavenParser.ExEnvironment {
    public class NativeFunction {
        protected MethodInfo method;
        protected string name;
        protected int numParams;
        public NativeFunction(string n, MethodInfo m) {
            name = n;
            method = m;
            numParams = m.GetParameters().Length;
        }
        public override string ToString() {
            return "<native:" + GetHashCode() + ">";
        }
        public int NumOfParameters {
            get {
                return numParams;
            }
        }
        public object Invoke(object[] args, ASTree tree) {
            if (method == null) return new EvalException("bad native function call: " + name, tree);
            try {
                return method.Invoke(null, args);
            }
            catch (Exception) {
                return new EvalException("bad native function call: " + name, tree);
            }
        }
    }
}
