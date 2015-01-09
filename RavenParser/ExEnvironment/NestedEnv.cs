using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
namespace RavenParser.ExEnvironment {
    public class NestedEnv : IEnvironment {
        protected Dictionary<string, object> values;
        protected IEnvironment outer;
        public NestedEnv() : this(null) { }
        public NestedEnv(IEnvironment e) {
            values = new Dictionary<string, object>();
            outer = e;
        }
        public void SetOuter(IEnvironment e) {
            outer = e;
        }

        public void PutNew(string name, object value) {
            if (name == null) return;
            if (values.ContainsKey(name)) {
                values[name] = value;
            }
            else {
                values.Add(name, value);
            }
        }

        public IEnvironment Where(string name) {
            if (name == null) return null;
            if (values.ContainsKey(name)) {
                return this;
            }
            else if (outer == null) {
                return null;
            }
            else {
                return ((NestedEnv)outer).Where(name);
            }
        }

        #region IEnvironment 成员

        public void Put(string name, object value) {
            IEnvironment e = Where(name);
            if (e == null) {
                e = this;
            }
            ((NestedEnv)e).PutNew(name, value);
        }

        public object Get(string name) {
            object v = null;
            if (name == null) return v;
            if (values.ContainsKey(name)) {
                v = values[name];
            }
            if (v == null && outer != null) {
                return outer.Get(name);
            }
            return v;
        }

        #endregion
    }
}
