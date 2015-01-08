using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExAST;
namespace RavenParser.ExEnvironment {
    public class Function {
        protected ParameterList parameters;
        protected BlockStmt body;
        protected IEnvironment env;
        public Function(ParameterList parameters, BlockStmt body, IEnvironment env) {
            this.parameters = parameters;
            this.body = body;
            this.env = env;
        }
        public ParameterList Parameters {
            get {
                return parameters;
            }
        }
        public BlockStmt Body {
            get {
                return body;
            }
        }
        public IEnvironment MakeEnv() {
            return new NestedEnv(env);
        }
        public override string ToString() {
            return "<fun:" + GetHashCode() + ">";
        }
    }
}
