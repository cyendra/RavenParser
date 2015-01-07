using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace RavenParser.BaseParser {
    public abstract class ASTree : IEnumerable<ASTree> {
        public static readonly string VisitMethod = "Visit";
        public virtual ASTree this[int index] {
            get {
                return null;
            }
        }

        public virtual int NumChildern {
            get {
                return 0;
            }
        }
        public abstract string Location();

        public void Accept(object visitor,IEnvironment env) {
            MethodInfo method = FindMethod(visitor, GetType());
            if (method != null) {
                method.Invoke(visitor, new object[] { this, env });
            }
        }
        private static MethodInfo FindMethod(object visitor, Type type) {
            if (type == typeof(object)) return null;
            MethodInfo method = visitor.GetType().GetMethod(VisitMethod, new Type[] { type, typeof(IEnvironment) });
            if (method != null) return method;
            else return FindMethod(visitor, type.BaseType);
        }

        #region IEnumerable<ASTree> 成员

        public virtual IEnumerator<ASTree> GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
