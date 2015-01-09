using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RavenParser.BaseParser;
using RavenParser.ExException;

namespace RavenParser.ExEnvironment {
    public class Natives {
        public IEnvironment Enviroment(IEnvironment env) {
            AppendNatives(env);
            return env;
        }
        protected void AppendNatives(IEnvironment env) {
            Append(env, "Write", typeof(Natives), "Write", typeof(object));
            Append(env, "Strlen", typeof(Natives), "Strlen", typeof(string));
            Append(env, "ToInt", typeof(Natives), "ToInt", typeof(object));
        }
        protected void Append(IEnvironment env, string name, Type clazz, string methodName, params Type[] types) {
            MethodInfo m;
            m = clazz.GetMethod(methodName, types);
            env.Put(name, new NativeFunction(methodName, m));
        }
        public static int Write(object obj) {
            System.Console.WriteLine(obj.ToString());
            return 0;
        }
        public static int Strlen(string s) {
            return s.Length;
        }
        public static int ToInt(object value) {
            int res;
            try {
                res = int.Parse(value.ToString());
            }
            catch (Exception) {
                res = 0;
            }
            return res;
        }
    }
}
