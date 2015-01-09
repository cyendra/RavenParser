using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExAST;
namespace RavenParser.ExEnvironment {
    public class RavObject {
        public class AccessException : Exception { }
        protected IEnvironment env;
        public RavObject(IEnvironment e) {
            env = e;
        }
        public override string ToString() {
            return "<object:" + GetHashCode() + ">";
        }
        public object Read(string member) {
            return GetEnv(member).Get(member);
        }
        public void Write(string member, object value) {
            GetEnv(member).PutNew(member, value);
        }
        protected IEnvironment GetEnv(string member) {
            IEnvironment e = env.Where(member);
            if (e != null && e == env) {
                return e;
            }
            else {
                throw new AccessException();
            }
        }
    }
}
