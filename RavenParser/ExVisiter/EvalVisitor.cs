using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RavenParser.BaseParser;
using RavenParser.ExAST;
using RavenParser.ExException;
namespace RavenParser.ExVisiter {
    public class EvalVisitor {
        private object result;
        public object Result {
            get {
                return result;
            }
        }
        public void Visit(ASTree t, IEnvironment env) {
            throw new EvalException("cannot eval: [ASTree]");
        }
        public void Visit(ASTList t, IEnvironment env) {
            throw new EvalException("cannot eval: " + t.ToString());
        }
        public void Visit(ASTLeaf t, IEnvironment env) {
            throw new EvalException("cannot eval: " + t.ToString());
        }
        public void Visit(IntegerLiteral t, IEnvironment env) {
            result = t.Value;
            return;
        }
        public void Visit(StringLiteral t, IEnvironment env) {
            result = t.Value;
            return;
        }
        public void Visit(Name t, IEnvironment env) {
            object value = env.Get(t.Text);
            if (value == null) {
                throw new EvalException("undefined name: " + t.Text, t);
            }
            else {
                result = value;
                return;
            }
        }
        public void Visit(NegativeExpr t, IEnvironment env) {
            Visit(t.Operand, env);
            object v = result;
            if (v is int) {
                int rs = -((int)v);
                result = rs;
                return;
            }
            else {
                throw new EvalException("bad type for -", t);
            }
        }
        public void Visit(BinaryExpr t, IEnvironment env) {
            string op = t.Operator;
            if (op == ":=") {
                Visit(t.Right, env);
                object right = result;
                result = ComputeAssign(t, env, right);
                return;
            }
            else {
                Visit(t.Left, env);
                object left = result;
                Visit(t.Right, env);
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
            else {
                throw new EvalException("bad assignment", t);
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
                throw new EvalException("bad type", t);
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
                throw new EvalException("bad operator", t);
            }
        }
        public void Visit(BlockStmt t, IEnvironment env) {
            object res = 0;
            foreach (ASTree tree in t) {
                if (!(tree is NullStmt)) {
                    Visit(tree, env);
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
            Visit(t.Condition, env);
            object c = result;
            if (ObjectIsTrue(c)) {
                Visit(t.ThenBlock, env);
                return;
            }
            else {
                if (t.ElseBlock == null) {
                    result = 0;
                    return;
                }
                else {
                    Visit(t.ElseBlock, env);
                    return;
                }
            }
        }
        public void Visit(WhileStmt t, IEnvironment env) {
            object res = 0;
            for (; ; ) {
                Visit(t.Condition, env);
                object c = result;
                if (ObjectIsTrue(c)) {
                    Visit(t.Body, env);
                    res = result;
                }
                else {
                    result = res;
                    return;
                }
            }
        }
    }
}
