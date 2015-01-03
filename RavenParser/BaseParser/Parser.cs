using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RavenParser.BaseAST;
using RavenParser.BaseLexer;
using RavenParser.BaseToken;

namespace RavenParser.BaseParser {

    #region Exception

    public class ParseException : Exception {
        public ParseException(Token t) : this("", t) { }
        public ParseException(string msg, Token t) : base("syntax error around " + Location(t) + ". " + msg) { }
        private static string Location(Token t) {
            if (t == Token.EOF) {
                return "the last line";
            }
            else {
                return "\"" + t.Text + "\" at line " + t.LineNumber;
            }
        }
        public ParseException(Exception e) : base("", e) { }
        public ParseException(string msg) : base(msg) { }
    }

    #endregion

    #region Element

    abstract class Element {
        public abstract void Parse(ILexer lexer, List<ASTree> res);
        public abstract bool Match(ILexer lexer);
    }

    class Tree : Element {
        private Parser parser;
        public Tree(Parser p) {
            parser = p;
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            res.Add(parser.parse(lexer));
        }
        public override bool Match(ILexer lexer) {
            return parser.Match(lexer);
        }
    }

    class OrTree : Element {
        private Parser[] parsers;
        public OrTree(Parser[] p) {
            parsers = p;
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            Parser p = choose(lexer);
            if (p == null) {
                throw new ParseException(lexer.Peek(0));
            }
            else {
                res.Add(p.parse(lexer));
            }
        }
        public override bool Match(ILexer lexer) {
            return choose(lexer) != null;
        }
        public void Insert(Parser p) {
            Parser[] newParsers = new Parser[parsers.Length + 1];
            newParsers[0] = p;
            Array.Copy(parsers, 0, newParsers, 1, parsers.Length);
            parsers = newParsers;
        }
        private Parser choose(ILexer lexer) {
            foreach (var p in parsers) {
                if (p.Match(lexer)) {
                    return p;
                }
            }
            return null;
        }
    }

    class Repeat : Element {
        private Parser parser;
        private bool onlyOnce;
        public Repeat(Parser p, bool once) {
            parser = p;
            onlyOnce = once;
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            while (parser.Match(lexer)) {
                ASTree t = parser.parse(lexer);
                if (t.GetType() != typeof(ASTList) || t.NumChildern > 0) {
                    res.Add(t);
                }
                if (onlyOnce) {
                    break;
                }
            }
        }
        public override bool Match(ILexer lexer) {
            return parser.Match(lexer);
        }
    }

    abstract class AToken : Element {
        private Factory factory;
        public AToken(Type type) {
            if (type == null) {
                type = typeof(ASTLeaf);
            }
            factory = Factory.Get(type, typeof(Token));
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            Token t = lexer.Read();
            if (Test(t)) {
                ASTree leaf = factory.Make(t);
                res.Add(leaf);
            }
            else {
                throw new ParseException(t);
            }
        }
        public override bool Match(ILexer lexer) {
            return Test(lexer.Peek(0));
        }
        protected abstract bool Test(Token t);
    }

    class IdToken : AToken {
        private HashSet<string> reserved;
        public IdToken(Type type, HashSet<string> r) : base(type) {
            reserved = r != null ? r : new HashSet<string>();
        }
        protected override bool Test(Token t) {
            return t.IsIdentifier && !reserved.Contains(t.Text);
        }
    }

    class IntegerToken : AToken {
        public IntegerToken(Type type) : base(type) { }
        protected override bool Test(Token t) {
            return t.IsInteger;
        }
    }

    class RealToken : AToken {
        public RealToken(Type type) : base(type) { }
        protected override bool Test(Token t) {
            return t.IsFloat;
        }
    }

    class StrToken : AToken {
        public StrToken(Type type) : base(type) { }
        protected override bool Test(Token t) {
            return t.IsFloat;
        }
    }

    class Leaf : Element {
        private string[] tokens;
        public Leaf(string[] pat) {
            tokens = pat;
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            Token t = lexer.Read();
            if (t.IsIdentifier) {
                foreach (var token in tokens) {
                    if (token == t.Text) {
                        Find(res, t);
                        return;
                    }
                }
            }
            if (tokens.Length > 0) {
                throw new ParseException(tokens[0] + " expected.", t);
            }
            else {
                throw new ParseException(t);
            }
        }
        public override bool Match(ILexer lexer) {
            Token t = lexer.Peek(0);
            if (t.IsIdentifier) {
                foreach (var token in tokens) {
                    if (token == t.Text) {
                        return true;
                    }
                }
            }
            return false;
        }
        protected virtual void Find(List<ASTree> res, Token t) {
            res.Add(new ASTLeaf(t));
        }
    }

    class Skip : Leaf {
        public Skip(string[] t) : base(t) { }
        protected override void Find(List<ASTree> res, Token t) { }
    }

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

    class Expr : Element {
        private Factory factory;
        private Operators ops;
        private Parser factor;
        public Expr(Type clazz, Parser exp, Operators map) {
            factory = Factory.GetForASTList(clazz);
            ops = map;
            factor = exp;
        }
        public override void Parse(ILexer lexer, List<ASTree> res) {
            ASTree right = factor.parse(lexer);
            Precedence prec;
            while ((prec = NextOperator(lexer)) != null) {
                right = DoShift(lexer, right, prec.Value);
            }
            res.Add(right);
        }
        public override bool Match(ILexer lexer) {
            return factor.Match(lexer);
        }
        private ASTree DoShift(ILexer lexer, ASTree left, int prec) {
            List<ASTree> list = new List<ASTree>();
            list.Add(left);
            list.Add(new ASTLeaf(lexer.Read()));
            ASTree right = factor.parse(lexer);
            Precedence next;
            while ((next = NextOperator(lexer)) != null && RightIsExpr(prec,next)) {
                right = DoShift(lexer, right, next.Value);
            }
            list.Add(right);
            return factory.Make(list);
        }
        private Precedence NextOperator(ILexer lexer) {
            Token t = lexer.Peek(0);
            if (t.IsIdentifier) {
                return ops[t.Text];
            }
            else {
                return null;
            }
        }
        private static bool RightIsExpr(int prec, Precedence nextPrec) {
            if (nextPrec.LeftAssoc) {
                return prec < nextPrec.Value;
            }
            else {
                return prec <= nextPrec.Value;
            }
        }
        
    }

    #endregion

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
