using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RavenParser.BaseAST;
using RavenParser.BaseLexer;
using RavenParser.BaseParser.Elemets;

namespace RavenParser.BaseParser {

    public class Precedence {
        public int Value {
            get;
            set;
        }
        public bool LeftAssoc {
            get;
            set;
        }
        public Precedence(int v, bool a) {
            Value = v;
            LeftAssoc = a;
        }
    }

    public class Operators : Dictionary<string, Precedence> {
        public static bool LEFT = true;
        public static bool RIGHT = false;
        public void Add(string name, int prec, bool leftAssoc) {
            Add(name, new Precedence(prec, leftAssoc));
        }
    }

    #region Factory
    
    class Factory {
        
        public static readonly string FactoryName = "Create";

        protected delegate ASTree MakeDelegate(object arg);
        protected MakeDelegate make0;
        protected Factory(MakeDelegate make) {
            make0 = make;
        }
        public ASTree Make(object arg) {
            return make0(arg);
        }
        public static Factory GetForASTList(Type clazz) {
            Factory f = Get(clazz, typeof(List<ASTree>));
            if (f == null) {
                f = new Factory(delegate(object arg) {
                    List<ASTree> results = (List<ASTree>)arg;
                    if (results.Count == 1) {
                        return results[0];
                    }
                    else {
                        return new ASTList(results);
                    }
                });
            }
            return f;
        }
        public static Factory Get(Type clazz, Type argType) {
            if (clazz == null) {
                return null;
            }
            MethodInfo method = clazz.GetMethod(FactoryName, 
                BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, 
                null, 
                new Type[] { argType }, 
                null);
            if (method != null) {
                MakeDelegate make = delegate(object arg) {
                    return method.Invoke(null, new object[] { arg }) as ASTree;
                };
                return new Factory(make);
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

    #endregion

    public class Parser {

        private List<Element> elements;
        private Factory factory;

        #region Parser

        public Parser(Type clazz) {
            reset(clazz);
        }
        protected Parser(Parser p) {
            elements = p.elements;
            factory = p.factory;
        }

        public ASTree parse(ILexer lexer) {
            List<ASTree> results = new List<ASTree>();
            foreach (var e in elements) {
                e.Parse(lexer, results);
            }
            return factory.Make(results);
        }

        public bool Match(ILexer lexer) {
            if (elements.Count == 0) {
                return true;
            }
            else {
                Element e = elements[0];
                return e.Match(lexer);
            }
        }

        public static Parser rule() {
            return rule(null);
        }
        public static Parser rule(Type clazz) {
            return new Parser(clazz);
        }

        public Parser reset() {
            elements = new List<Element>();
            return this;
        }
        public Parser reset(Type clazz) {
            elements = new List<Element>();
            factory = Factory.GetForASTList(clazz);
            return this;
        }

        public Parser integer() {
            return integer(null);
        }
        public Parser integer(Type clazz) {
            elements.Add(new IntegerToken(clazz));
            return this;
        }

        public Parser real() {
            return real(null);
        }
        public Parser real(Type clazz) {
            elements.Add(new RealToken(clazz));
            return this;
        }

        public Parser identifier(HashSet<string> reserved) {
            return identifier(null, reserved);
        }
        public Parser identifier(Type clazz, HashSet<string> reserved) {
            elements.Add(new IdToken(clazz, reserved));
            return this;
        }

        public Parser str() {
            return str(null);
        }
        public Parser str(Type clazz) {
            elements.Add(new StrToken(clazz));
            return this;
        }

        public Parser token(params string[] pat) {
            elements.Add(new Leaf(pat));
            return this;
        }
        public Parser sep(params string[] pat) {
            elements.Add(new Skip(pat));
            return this;
        }
        public Parser ast(Parser p) {
            elements.Add(new Tree(p));
            return this;
        }
        public Parser or(params Parser[] p) {
            elements.Add(new OrTree(p));
            return this;
        }
        public Parser maybe(Parser p) {
            Parser p2 = new Parser(p);
            p2.reset();
            elements.Add(new OrTree(new Parser[] { p, p2 }));
            return this;
        }
        public Parser option(Parser p) {
            elements.Add(new Repeat(p, true));
            return this;
        }
        public Parser repeat(Parser p) {
            elements.Add(new Repeat(p, false));
            return this;
        }
        public Parser expression(Parser subexp, Operators operators) {
            elements.Add(new Expr(null, subexp, operators));
            return this;
        }
        public Parser expression(Type clazz, Parser subexp, Operators operators) {
            elements.Add(new Expr(clazz, subexp, operators));
            return this;
        }
        public Parser insertChoice(Parser p) {
            Element e = elements[0];
            if (e is OrTree) {
                ((OrTree)e).Insert(p);
            }
            else {
                Parser otherwise = new Parser(this);
                reset(null);
                or(p, otherwise);
            }
            return this;
        }

        #endregion



    }
}
