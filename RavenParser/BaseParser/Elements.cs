using RavenParser.BaseAST;
using RavenParser.BaseLexer;
using RavenParser.BaseToken;
using System;
using System.Collections.Generic;
using RavenParser.BaseException;
namespace RavenParser.BaseParser {
    namespace Elemets {

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
            public IdToken(Type type, HashSet<string> r)
                : base(type) {
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
                while ((next = NextOperator(lexer)) != null && RightIsExpr(prec, next)) {
                    right = DoShift(lexer, right, next.Value);
                }
                list.Add(right);
                return factory.Make(list);
            }
            private Precedence NextOperator(ILexer lexer) {
                Token t = lexer.Peek(0);
                if (t.IsIdentifier) {
                    if (ops.ContainsKey(t.Text)) return ops[t.Text];
                    return null;
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

    }
}
