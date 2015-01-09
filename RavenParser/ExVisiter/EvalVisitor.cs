using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RavenParser.BaseParser;
using RavenParser.ExAST;
using RavenParser.ExException;
using RavenParser.ExEnvironment;
namespace RavenParser.ExVisiter {
    public class ErrorValue {
        private string errMsg;
        public ErrorValue(string s) {
            errMsg = s;
        }
        public ErrorValue(string s, ASTree t) : this(s + " " + t.Location()) { }
        public string Message {
            get {
                return errMsg;
            }
        }
        public override string ToString() {
            return errMsg;
        }
    }
    public class EvalVisitor {
        private bool debug;
        public bool DebugOption {
            get {
                return debug;
            }
            set {
                debug = value;
            }
        }
        private object result;
        public object Result {
            get {
                return result;
            }
        }
        public EvalVisitor() {
            debug = false;
            result = 0;
        }
        public void Visit(ASTree t, IEnvironment env) {
            if (debug) System.Console.WriteLine("ASTree: " + t.GetType().ToString());
            result = new ErrorValue("cannot eval: [ASTree]");
            return;
        }
        public void Visit(ASTList t, IEnvironment env) {
            if (debug) System.Console.WriteLine("ASTList: " + t.GetType().ToString());
            result = new ErrorValue("cannot eval: " + t.ToString());
            return;
        }
        public void Visit(ASTLeaf t, IEnvironment env) {
            if (debug) System.Console.WriteLine("ASTLeaf: " + t.GetType().ToString());
            result = new ErrorValue("cannot eval: " + t.ToString());
            return;
        }
        public void Visit(IntegerLiteral t, IEnvironment env) {
            if (debug) System.Console.WriteLine("IntegerLiteral: " + t.GetType().ToString());
            result = t.Value;
            return;
        }
        public void Visit(StringLiteral t, IEnvironment env) {
            if (debug) System.Console.WriteLine("StringLiteral: " + t.GetType().ToString());
            result = t.Value;
            return;
        }
        public void Visit(Name t, IEnvironment env) {
            if (debug) System.Console.WriteLine("Name: " + t.GetType().ToString());
            object value = env.Get(t.Text);
            if (value == null) {
                result = new ErrorValue("undefined name: " + t.Text, t);
                return;
            }
            else {
                result = value;
                return;
            }
        }
        public void Visit(NegativeExpr t, IEnvironment env) {
            if (debug) System.Console.WriteLine("NegativeExpr: " + t.GetType().ToString());
            t.Operand.Accept(this, env);
            if (result is ErrorValue) return;
            object v = result;
            if (v is int) {
                int rs = -((int)v);
                result = rs;
                return;
            }
            else {
                result = new ErrorValue("bad type for -", t);
                return;
            }
        }
        public void Visit(BinaryExpr t, IEnvironment env) {
            if (debug) System.Console.WriteLine("BinaryExpr: " + t.GetType().ToString());
            string op = t.Operator;
            if (op == ":=") {
                t.Right.Accept(this, env);
                if (result is ErrorValue) return;
                object right = result;
                result = ComputeAssign(t, env, right);
                return;
            }
            else {
                t.Left.Accept(this, env);
                if (result is ErrorValue) return;
                object left = result;
                t.Right.Accept(this, env);
                if (result is ErrorValue) return;
                object right = result;
                result = ComputeOp(t, left, op, right);
                return;
            }
        }
        private object ComputeAssign(BinaryExpr t, IEnvironment env, object rvalue) {
            ASTree l = t.Left;
            if (l is Name) {
                env.Put((l as Name).Text, rvalue);
                return rvalue;
            }
            else if (l is PrimaryExpr) {
                PrimaryExpr p = l as PrimaryExpr;
                if (p.HasPostfix(0) && (p.Postfix(0) is Dot)) {
                    p.Accept(this, env, 1);
                    object tp = result;
                    if (result is ErrorValue) return result;
                    if (tp is RavObject) {
                        return SetField(t, (tp as RavObject), (p.Postfix(0) as Dot), rvalue);
                    }
                }
            }
            return new ErrorValue("bad assignment", t);
        }
        private object SetField(BinaryExpr t, RavObject obj, Dot expr, object rvalue) {
            string name = expr.Name;
            try {
                obj.Write(name, rvalue);
                return rvalue;
            }
            catch (Exception) {
                return new ErrorValue("bad member access " + t.Location() + " in Assgin: " + name);
            }
        }
        private object ComputeOp(BinaryExpr t, object left, string op, object right) {
            if (left is int && right is int) {
                return ComputeInteger(t, (int)left, op, (int)right);
            }
            if (op == "+") {
                return left as string + right as string;
            }
            else if (op == "=") {
                if (left == null) {
                    return right == null ? true : false;
                }
                else {
                    return left.Equals(right) ? true : false;
                }
            }
            else {
                return new ErrorValue("bad type", t);
            }
        }
        private object ComputeInteger(BinaryExpr t, int left, string op, int right) {
            int a = left;
            int b = right;
            if (op == "+") {
                return a + b;
            }
            else if (op == "-") {
                return a - b;
            }
            else if (op == "*") {
                return a * b;
            }
            else if (op == "/") {
                return a / b;
            }
            else if (op == "%") {
                return a % b;
            }
            else if (op == "=") {
                return a == b;
            }
            else if (op == ">") {
                return a > b;
            }
            else if (op == "<") {
                return a < b;
            }
            else if (op == ">=") {
                return a >= b;
            }
            else if (op == "<=") {
                return a <= b;
            }
            else {
                return new ErrorValue("bad operator", t);
            }
        }
        public void Visit(BlockStmt t, IEnvironment env) {
            if (debug) System.Console.WriteLine("BlockStmt: " + t.GetType().ToString());
            object res = 0;
            foreach (ASTree tree in t) {
                if (!(tree is NullStmt)) {
                    tree.Accept(this, env);
                    if (result is ErrorValue) return;
                    res = result;
                }
            }
            result = res;
            return;
        }
        private bool ObjectIsTrue(object c) {
            if ((c is int && (int)c == 0) || (c is bool && (bool)c == false) || c == null) return false;
            return true;
        }
        public void Visit(IfStmt t, IEnvironment env) {
            if (debug) System.Console.WriteLine("IfStmt: " + t.GetType().ToString());
            t.Condition.Accept(this, env);
            if (result is ErrorValue) return;
            object c = result;
            if (ObjectIsTrue(c)) {
                t.ThenBlock.Accept(this, env);
                if (result is ErrorValue) return;
                return;
            }
            else {
                if (t.ElseBlock == null) {
                    result = 0;
                    return;
                }
                else {
                    t.ElseBlock.Accept(this, env);
                    if (result is ErrorValue) return;
                    return;
                }
            }
        }
        public void Visit(WhileStmt t, IEnvironment env) {
            if (debug) System.Console.WriteLine("WhileStmt: " + t.GetType().ToString());
            object res = 0;
            for (; ; ) {
                t.Condition.Accept(this, env);
                if (result is ErrorValue) return;
                object c = result;
                if (ObjectIsTrue(c)) {
                    t.Body.Accept(this, env);
                    if (result is ErrorValue) return;
                    res = result;
                }
                else {
                    result = res;
                    return;
                }
            }
        }
        public void Visit(DefStmt t, IEnvironment env) {
            env.PutNew(t.Name, new Function(t.Parameters, t.Body, env));
            result = t.Name;
            return;
        }
        /*
        public void Visit(PrimaryExpr t, IEnvironment env) {
            t.Operand.Accept(this, env);
            object res = result;
            if (res is ErrorValue) return;
            int n = t.NumChildern;
            for (int i = 0; i < n - 1; i++) {
                t.Postfix(i).Accept(this, env, res);
                if (result is ErrorValue) return;
            }
            return;
        }
        */
        public void Visit(PrimaryExpr t, IEnvironment env) {
            EvalSubExpr(t, env, 0);
        }
        public void Visit(PrimaryExpr t, IEnvironment env, int nest) {
            EvalSubExpr(t, env, nest);
        }
        private object EvalSubExpr(PrimaryExpr t, IEnvironment env, int nest) {
            if (t.HasPostfix(nest)) {
                object target = EvalSubExpr(t, env, nest + 1);
                t.Postfix(nest).Accept(this, env, target);
                return result;
            }
            else {
                t.Operand.Accept(this, env);
                return result;
            }
        }
        public void Visit(Postfix t, IEnvironment env, object value) {
            result = new ErrorValue("No Impl Postfix");
            return;
        }
        public void Visit(Arguments t, IEnvironment callerEnv, object value) {
            if (value is NativeFunction) {
                NativeFunction func = value as NativeFunction;
                int nparams = func.NumOfParameters;
                if (t.Size != nparams) {
                    result = new ErrorValue("bad number of arguments", t);
                    return;
                }
                object[] args = new object[nparams];
                int num = 0;
                foreach (ASTree a in t) {
                    a.Accept(this, callerEnv);
                    if (result is ErrorValue) return;
                    args[num++] = result;
                }
                result = func.Invoke(args, t);
                return;
            }
            if (value is Function) {
                Function func = value as Function;
                ParameterList pars = func.Parameters;
                if (t.Size != pars.Size) {
                    result = new ErrorValue("bad number of arguments", t);
                    return;
                }
                IEnvironment newEnv = func.MakeEnv();
                int num = 0;
                foreach (ASTree a in t) {
                    a.Accept(this, callerEnv);
                    if (result is ErrorValue) return;
                    pars.Accept(this, newEnv, num++, result);
                    if (result is ErrorValue) return;
                }
                func.Body.Accept(this, newEnv);
                return;
            }
            result = new ErrorValue("bad function", t);
            return;
        }
        public void Visit(ParameterList t, IEnvironment env, int index, object value) {
            env.PutNew(t.Name(index), value);
        }
        public void Visit(Lambda t, IEnvironment env) {
            result = new Function(t.Parameters, t.Body, env);
        }
        public void Visit(ClassStmt t, IEnvironment env) {
            ClassInfo ci;
            try {
                ci = new ClassInfo(t, env);
            }
            catch (Exception e) {
                result = new ErrorValue(e.Message);
                return;
            }
            env.Put(t.Name, ci);
            result = t.Name;
            return;
        }
        public void Visit(ClassBody t, IEnvironment env) {
            foreach (ASTree tree in t) {
                tree.Accept(this, env);
                if (result is ErrorValue) return;
            }
            result = 0;
            return;
        }
        public void Visit(Dot t, IEnvironment env, object value) {
            string member = t.Name;
            if (value is ClassInfo) {
                if (member == "new") {
                    ClassInfo ci = value as ClassInfo;
                    NestedEnv e = new NestedEnv(ci.Environment);
                    RavObject ro = new RavObject(e);
                    e.PutNew("this", ro);
                    InitObject(ci, e);
                    result = ro;
                    return;
                }
            }
            else if (value is RavObject) {
                try {
                    result = (value as RavObject).Read(member);
                    return;
                }
                catch (RavObject.AccessException) { }
            }
            result = new ErrorValue("bad member access in Dot: " + member, t);
        }
        private void InitObject(ClassInfo ci, IEnvironment env) {
            if (ci.SuperClass != null) {
                InitObject(ci.SuperClass, env);
                if (result is ErrorValue) return;
            }
            ci.Body.Accept(this, env);
        }
    }
}
