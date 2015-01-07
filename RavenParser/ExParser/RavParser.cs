using RavenParser.ExAST;
using RavenParser.ExLexer;
using RavenParser.BaseParser;
using System.Collections.Generic;
namespace RavenParser.ExParser {
    public class RavParser {
        private HashSet<string> reserved;
        private Operators op;
        private Parser expr0;
        private Parser primary;
        private Parser factor;
        private Parser expr;
        private Parser statement0;
        private Parser block;
        private Parser simple;
        private Parser statement;
        private Parser program;
/*  primary     : "(" expr ")" | INTEGER | IDENTIFIER | STRING
 *  factor      : "-" primary | primary
 *  expr        : factor { OP factor }
 *  block       : "begin" [ statement ] { ";" [statement] } "end"
 *  simple      : expr
 *  statement   : "if" expr "then" statement [ "else" statement ] 
 *              | "while" expr "do" statement 
 *              | simple 
 *              | block 
 *  program     : [ statement ] ;
 */
        private void RavBNF() {
            expr0 = Parser.rule();
            primary = Parser.rule(typeof(PrimaryExpr)).or(Parser.rule().sep("(").ast(expr0).sep(")"),
                                                          Parser.rule().integer(typeof(IntegerLiteral)),
                                                          Parser.rule().identifier(typeof(Name), reserved),
                                                          Parser.rule().str(typeof(StringLiteral)));
            factor = Parser.rule().or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary), primary);
            expr = expr0.expression(typeof(BinaryExpr), factor, op);
            simple = Parser.rule(typeof(PrimaryExpr)).ast(expr);
            statement0 = Parser.rule();
            block = Parser.rule(typeof(BlockStmt)).sep("begin")
                                                  .option(statement0)
                                                  .repeat(Parser.rule().sep(";").option(statement0))
                                                  .sep("end");
            statement = statement0.or(
                Parser.rule(typeof(IfStmt)).sep("if").ast(expr).sep("then").ast(statement0)
                                           .option(Parser.rule().sep("else").ast(statement0)),
                Parser.rule(typeof(WhileStmt)).sep("while").ast(expr).sep("do").ast(statement0),
                simple,
                block);
            program = Parser.rule().or(Parser.rule().option(statement), Parser.rule(typeof(NullStmt))).sep(";");
        }

        private void StoneBNF() {
            expr0 = Parser.rule();
            primary = Parser.rule(typeof(PrimaryExpr)).or(Parser.rule().sep("(").ast(expr0).sep(")"),
                                                          Parser.rule().integer(typeof(IntegerLiteral)),
                                                          Parser.rule().identifier(typeof(Name), reserved),
                                                          Parser.rule().str(typeof(StringLiteral)));
            factor = Parser.rule().or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary), primary);
            expr = expr0.expression(typeof(BinaryExpr), factor, op);
            statement0 = Parser.rule();
            block = Parser.rule(typeof(BlockStmt)).sep("begin").option(statement0)
                                                  .repeat(Parser.rule().sep(";").option(statement0))
                                                  .sep("end");
            simple = Parser.rule(typeof(PrimaryExpr)).ast(expr);
            statement = statement0.or(
                Parser.rule(typeof(IfStmt)).sep("if").ast(expr).sep("then").ast(block)
                                           .option(Parser.rule().sep("else").ast(block)),
                Parser.rule(typeof(WhileStmt)).sep("while").ast(expr).sep("do").ast(block),
                simple);
            program = Parser.rule().or(statement, Parser.rule(typeof(NullStmt))).sep(";");
        }

        private void InitParser() {
            reserved = new HashSet<string>() { ";", "begin", "end", "if", "then", "else", "while", "do" };
            op = new Operators();
            op.Add(":=", 1, Operators.RIGHT);
            op.Add("=", 2, Operators.LEFT);
            op.Add(">", 2, Operators.LEFT);
            op.Add("<", 2, Operators.LEFT);
            op.Add("+", 3, Operators.LEFT);
            op.Add("-", 3, Operators.LEFT);
            op.Add("*", 4, Operators.LEFT);
            op.Add("/", 4, Operators.LEFT);
            op.Add("%", 4, Operators.LEFT);
        }
        public RavParser() {
            InitParser();
            RavBNF();
        }
        public ASTree Parse(Lexer lexer) {
            return program.parse(lexer);
        }
    }
}
