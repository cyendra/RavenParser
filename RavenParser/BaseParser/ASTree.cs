using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RavenParser.ExException;
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
        private static MethodInfo FindMethod(object visitor, Type clazz) {
            if (clazz == typeof(object)) return null;
            MethodInfo method = visitor.GetType().GetMethod(VisitMethod, new Type[] { clazz, typeof(IEnvironment) });
            if (method != null) return method;
            else return FindMethod(visitor, clazz.BaseType);
        }

        public void Accept(object visitor, IEnvironment env, params object[] objs) {
            int m = objs.Length;
            Type[] types = new Type[m];
            for (int i = 0; i < m; i++) {
                types[i] = objs[i].GetType();
            }
            MethodInfo method = FindMethod(visitor, GetType(), types);
            if (method != null) {
                int n = objs.Length + 2;
                object[] o = new object[n];
                o[0] = this;
                o[1] = env;
                for (int i = 2; i < n; i++) {
                    o[i] = objs[i - 2];
                }
                method.Invoke(visitor, o);
            }
        }
        private static MethodInfo FindMethod(object visitor, Type clazz, Type[] objType) {
            if (clazz == typeof(object)) return null;
            int n = objType.Length + 2;
            Type[] types = new Type[n];
            types[0] = clazz;
            types[1] = typeof(IEnvironment);
            for (int i = 2; i < n; i++) {
                types[i] = objType[i - 2];
            }
            MethodInfo method = visitor.GetType().GetMethod(VisitMethod, types);
            if (method != null) return method;
            else return FindMethod(visitor, clazz.BaseType, objType);
            
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
