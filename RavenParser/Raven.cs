using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.BaseParser;
using RavenParser.ExLexer;
using RavenParser.ExParser;
using RavenParser.ExVisiter;
using RavenParser.ExAST;
using RavenParser.ExToken;
using RavenParser.ExEnvironment;
using System.IO;
namespace RavenParser {
    public class Raven {
        private Lexer lexer;
        private RavParser parser;
        private EvalVisitor visitor;
        private IEnvironment env;
        private TextReader inStream;
        private TextWriter outStream;
        private TextWriter errStream;
        public void Init() {
            lexer = new Lexer(inStream);
            parser = new RavParser();
            visitor = new EvalVisitor();
            env = new Natives().Enviroment(new NestedEnv());
        }
        public Raven() {
            inStream = System.Console.In;
            outStream = System.Console.Out;
            errStream = System.Console.Error;
            Init();
        }
        public Raven(TextReader sin, TextWriter sout, TextWriter eout) {
            inStream = sin;
            outStream = sout;
            errStream = eout;
            Init();
        }
        public Raven(string path) {
            inStream = new StreamReader(path);
            outStream = System.Console.Out;
            errStream = System.Console.Error;
            Init();
        }
        public void Run() {
            try {
                while (lexer.Peek(0) != Token.EOF) {
                    ASTree ast = parser.Parse(lexer);
                    ast.Accept(visitor, env);
                    if (visitor.Result is ErrorValue) {
                        errStream.WriteLine("Error > " + visitor.Result.ToString());
                    }
                    else {
                        errStream.WriteLine("> " + visitor.Result.ToString());
                    }
                    
                }
            }
            catch (ParseException ex) {
                errStream.WriteLine("Error > " + ex.Message);
            }    
        }
    }
}
