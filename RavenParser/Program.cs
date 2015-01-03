using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.Base;
using System.Reflection;
using RavenParser.BaseAST;
namespace RavenParser
{

    class ClsTest {
        public ClsTest(String t) {
            System.Console.WriteLine(t);
        }
        public ClsTest Gao(String t) {
            System.Console.WriteLine("-> " + t);
            return this;
        }
    }

    class Factory {
        protected delegate ASTree MakeDelegate(object arg);
        protected MakeDelegate make0;
        protected Factory(MakeDelegate make) {
            make0 = make;
        }
        public ASTree Make(object arg) {
            return make0(arg);
        }

        public static Factory get(Type clazz, Type argType) {
            if (clazz == null) {
                return null;
            }

            MethodInfo method = clazz.GetMethod("Create", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, new Type[] { argType }, null);
            if (method != null) {
                MakeDelegate make1 = delegate(object arg) {
                    return method.Invoke(null, new object[] { "createaa" }) as ASTree;
                };
                return new Factory(make1);
            }
            
            ConstructorInfo cons = clazz.GetConstructor(new Type[] { argType });
            if (cons != null) {
                MakeDelegate make = delegate(object arg) {
                    return cons.Invoke(new object[] { arg }) as ASTree;
                };
                return new Factory(make);
            }
            return null;
        }

    }

    class Func : ASTree {
        private string str;
        public Func(string s) {
            str = s;
        }
        public static ASTree Create(string s) {
            string str = "gogo create " + s;
            return new Func(str);
        }
        public override string Location() {
            System.Console.WriteLine(str);
            return str;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //BaseDebug debug = new BaseDebug();
            //debug.ShowAll();

            
            Factory factory = Factory.get(typeof(Func), typeof(string));
            var ast = factory.Make("aaa");
            ast.Location();
            

            /*
            Type type = typeof(ClsTest);
            var ins = Activator.CreateInstance(type, "gao ");

            ConstructorInfo cons = type.GetConstructor(new Type[] { typeof(string) });
            var user = cons.Invoke(new object[] { "手动 " });

            var n = type.InvokeMember("Gao", BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, user, new object[] { "wow" });
            */

            while (true) ;
        }
    }
}
