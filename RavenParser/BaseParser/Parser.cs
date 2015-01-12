using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RavenParser.BaseParser;
using RavenParser.ExLexer;
using RavenParser.BaseParser.Elemets;

namespace RavenParser.BaseParser {

    /// <summary>
    /// 运算符。
    /// </summary>
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

    /// <summary>
    /// 运算符表。
    /// </summary>
    public class Operators : Dictionary<string, Precedence> {
        public static bool LEFT = true;
        public static bool RIGHT = false;
        public void Add(string name, int prec, bool leftAssoc) {
            Add(name, new Precedence(prec, leftAssoc));
        }
    }

    #region Factory
    
    /// <summary>
    /// 反射工厂，不要使用。
    /// </summary>
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

    /// <summary>
    /// <para>Parser库将组合多个能够执行简单语法分析的对象，并以此新建一个能够执行比较复杂的语法分析的对象。</para>
    /// <para>库提供了一些基本的类，帮助实现用于执行语法分析的对象。</para>
    /// <para>库的使用者只要创建必需的对象，并将其组合，就能懂得所需的语法分析器。</para>
    /// </summary>
    public class Parser {

        private List<Element> elements;
        private Factory factory;

        /// <summary>
        /// 不要使用构造方法，而要使用 rule() 方法来创建 Parser 对象。
        /// </summary>
        /// <param name="clazz"></param>
        public Parser(Type clazz) {
            reset(clazz);
        }
        protected Parser(Parser p) {
            elements = p.elements;
            factory = p.factory;
        }

        /// <summary>
        /// <para>用指定的词法分析器做语法分析。</para>
        /// </summary>
        /// <param name="lexer">指定的词法分析器。</param>
        /// <returns></returns>
        public ASTree parse(ILexer lexer) {
            List<ASTree> results = new List<ASTree>();
            foreach (var e in elements) {
                e.Parse(lexer, results);
            }
            return factory.Make(results);
        }

        /// <summary>
        /// 不要用这个方法。
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public bool Match(ILexer lexer) {
            if (elements.Count == 0) {
                return true;
            }
            else {
                Element e = elements[0];
                return e.Match(lexer);
            }
        }

        /// <summary>
        /// <para>创建Parser对象。</para>
        /// <para>生成默认类型的节点。</para>
        /// </summary>
        /// <returns>创建的Parser对象</returns>
        public static Parser rule() {
            return rule(null);
        }
        /// <summary>
        /// <para>创建Parser对象。</para>
        /// <para>生成指定类型的节点。</para>
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <returns></returns>
        public static Parser rule(Type clazz) {
            return new Parser(clazz);
        }

        /// <summary>
        /// 清空语法规则。
        /// </summary>
        /// <returns></returns>
        public Parser reset() {
            elements = new List<Element>();
            return this;
        }
        /// <summary>
        /// 清空语法规则，指定新的节点类。
        /// </summary>
        /// <param name="clazz">节点类。</param>
        /// <returns></returns>
        public Parser reset(Type clazz) {
            elements = new List<Element>();
            factory = Factory.GetForASTList(clazz);
            return this;
        }

        /// <summary>
        /// 向语法规则中添加终结符（整型字面量）。
        /// </summary>
        /// <returns></returns>
        public Parser integer() {
            return integer(null);
        }
        /// <summary>
        /// 向语法规则中添加终结符（整型字面量），生成指定类型的节点。
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <returns></returns>
        public Parser integer(Type clazz) {
            elements.Add(new IntegerToken(clazz));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加终结符（实型字面量）。
        /// </summary>
        /// <returns></returns>
        public Parser real() {
            return real(null);
        }
        /// <summary>
        /// 向语法规则中添加终结符（实型字面量），生成指定类型的节点。
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <returns></returns>
        public Parser real(Type clazz) {
            elements.Add(new RealToken(clazz));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加终结符（除保留字r外的标识符）。
        /// </summary>
        /// <param name="reserved">保留字集合。</param>
        /// <returns></returns>
        public Parser identifier(HashSet<string> reserved) {
            return identifier(null, reserved);
        }
        /// <summary>
        /// 向语法规则中添加终结符（除保留字r外的标识符），生成指定类型的节点。
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <param name="reserved">保留字集合。</param>
        /// <returns></returns>
        public Parser identifier(Type clazz, HashSet<string> reserved) {
            elements.Add(new IdToken(clazz, reserved));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加终结符（字符串字面量）。
        /// </summary>
        /// <returns></returns>
        public Parser str() {
            return str(null);
        }
        /// <summary>
        /// 向语法规则中添加终结符（字符串字面量），生成指定类型的节点。
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <returns></returns>
        public Parser str(Type clazz) {
            elements.Add(new StrToken(clazz));
            return this;
        }
       
        /// <summary>
        /// 向语法规则中添加终结符（与 pat 匹配的标识符）。
        /// </summary>
        /// <param name="pat">要匹配的终结符。</param>
        /// <returns></returns>
        public Parser token(params string[] pat) {
            elements.Add(new Leaf(pat));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加未包含于抽象语法树的终结符（与 pat 匹配的标识符）。
        /// </summary>
        /// <param name="pat">要匹配的终结符。</param>
        /// <returns></returns>
        public Parser sep(params string[] pat) {
            elements.Add(new Skip(pat));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加非终结符 p。
        /// </summary>
        /// <param name="p">要匹配的非终结符。</param>
        /// <returns></returns>
        public Parser ast(Parser p) {
            elements.Add(new Tree(p));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加若干个由 or 关系连接的非终结符 p。
        /// </summary>
        /// <param name="p">多参数，或关系的非终结符。</param>
        /// <returns></returns>
        public Parser or(params Parser[] p) {
            elements.Add(new OrTree(p));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加可省略的非终结符 p（如果省略，则作为一棵仅有根节点的抽象语法树处理）。
        /// </summary>
        /// <param name="p">可省略的非终结符。</param>
        /// <returns></returns>
        public Parser maybe(Parser p) {
            Parser p2 = new Parser(p);
            p2.reset();
            elements.Add(new OrTree(new Parser[] { p, p2 }));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加可省略的非终结符 p。
        /// </summary>
        /// <param name="p">可省略的非终结符。</param>
        /// <returns></returns>
        public Parser option(Parser p) {
            elements.Add(new Repeat(p, true));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加至少重复 0 次的非终结符 p。
        /// </summary>
        /// <param name="p">至少重复 0 次的非终结符。</param>
        /// <returns></returns>
        public Parser repeat(Parser p) {
            elements.Add(new Repeat(p, false));
            return this;
        }

        /// <summary>
        /// 向语法规则中添加双目运算表达式。
        /// </summary>
        /// <param name="subexp">因子</param>
        /// <param name="operators">运算符表</param>
        /// <returns></returns>
        public Parser expression(Parser subexp, Operators operators) {
            elements.Add(new Expr(null, subexp, operators));
            return this;
        }
        /// <summary>
        /// 向语法规则中添加双目运算表达式，生成指定类型的节点。
        /// </summary>
        /// <param name="clazz">生成这种类型的节点。</param>
        /// <param name="subexp">因子。</param>
        /// <param name="operators">运算符表。</param>
        /// <returns></returns>
        public Parser expression(Type clazz, Parser subexp, Operators operators) {
            elements.Add(new Expr(clazz, subexp, operators));
            return this;
        }

        /// <summary>
        /// 为语法规则起始处的 or 添加新的分支选项。
        /// </summary>
        /// <param name="p">要添加的分支选项。</param>
        /// <returns></returns>
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

    }
}
