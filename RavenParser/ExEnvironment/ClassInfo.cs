using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExAST;
using RavenParser.ExException;
namespace RavenParser.ExEnvironment {
    public class ClassInfo {
        protected ClassStmt definition;
        protected IEnvironment environment;
        protected ClassInfo superClass;
        public ClassInfo(ClassStmt cs, IEnvironment env) {
            definition = cs;
            environment = env;
            object obj = cs.SuperClass != null ? env.Get(cs.SuperClass) : null;
            if (obj == null) {
                superClass = null;
            }
            else if (obj is ClassInfo) {
                superClass = obj as ClassInfo;
            }
            else {
                throw new EvalException("unknown super class: " + cs.SuperClass, cs);
            }
        }
        public string Name {
            get {
                return definition.Name;
            }
        }
        public ClassInfo SuperClass {
            get {
                return superClass;
            }
        }
        public ClassBody Body {
            get {
                return definition.Body;
            }
        }
        public IEnvironment Environment {
            get {
                return environment;
            }
        }
        public override string ToString() {
            return "<class " + Name + ">";
        }
    }
}
