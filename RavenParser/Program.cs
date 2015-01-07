using RavenParser.Base;
using RavenParser.BaseParser;
using RavenParser.ExForm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace RavenParser
{
    class Func {
        public void gao() {
            throw new Exception("ffff");
        }
    }
    class Ref {
        public void run(object obj, Type type) {
            MethodInfo method = type.GetMethod("gao");
            method.Invoke(obj, new object[] { });
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Input());
            
            //BaseDebug debug = new BaseDebug();
            //debug.RegexTest();
            //while (true) ;
        }
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            throw new Exception();

        }
    }
}
