using RavenParser.Base;
using RavenParser.BaseAST;
using RavenParser.BaseForm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RavenParser
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Input());

            BaseDebug debug = new BaseDebug();
            debug.RegexTest();
            
            while (true) ;
        }
    }
}
