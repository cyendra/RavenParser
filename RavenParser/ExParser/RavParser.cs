using RavenParser.ExAST;
using RavenParser.ExLexer;
using RavenParser.BaseParser;
using System.Collections.Generic;
namespace RavenParser.ExParser {
    public class RavParser {
        private HashSet<string> reserved;
        private Operators op;

        private Parser primary;
        private Parser factor;
        private Parser expr;
        private Parser block;
        private Parser simple;
        private Parser statement;
        private Parser program;
        private Parser param;
        private Parser pars;
        private Parser paramList;
        private Parser def;
        private Parser args;
        private Parser postfix;

        private Parser member;
        private Parser classBody;
        private Parser defclass;

        private Parser expr0;
        private Parser statement0;


        /*  primary     : "lambda" param_list block
         *              | ( "(" expr ")" | INTEGER | IDENTIFIER | STRING ) { postfix }
         *  factor      : "-" primary | primary
         *  expr        : factor { OP factor }
         *  block       : "begin" [ statement ] { ";" [statement] } "end"
         *  simple      : expr [ postfix ]
         *  statement   : "if" expr "then" statement [ "else" statement ] 
         *              | "while" expr "do" statement 
         *              | simple 
         *              | block 
         *  param       : IDENTIFIER
         *  params      : param { "," param }
         *  param_list  : "(" [ params ] ")"
         *  def         : "def" IDENTIFIER param_list block
         *  args        : expr { "," expr }
         *  postfix     : "(" [ args ] ")" 
         *              | "." IDENTIFIER
         *  member      : def | simple
         *  class_body  : "begin" [ member ] { ";" [member] } "end"
         *  defclass    : "class" IDENTIFIER [ "extends" IDENTiFIER ] class_body
         *  program     : [ statement | def | defclass ] ;
         */
        private void RavBNFWithClass() {
            primary = Parser.rule(typeof(PrimaryExpr));
            factor = Parser.rule();
            expr = Parser.rule();
            simple = Parser.rule(typeof(PrimaryExpr));
            block = Parser.rule(typeof(BlockStmt));
            statement = Parser.rule();
            program = Parser.rule();

            param = Parser.rule();
            pars = Parser.rule(typeof(ParameterList));
            paramList = Parser.rule();
            args = Parser.rule(typeof(Arguments));
            postfix = Parser.rule();
            def = Parser.rule(typeof(DefStmt));

            member = Parser.rule();
            classBody = Parser.rule(typeof(ClassBody));
            defclass = Parser.rule(typeof(ClassStmt));

            member = member.or(def, simple);
            classBody = classBody.sep("begin").option(member).repeat(Parser.rule().sep(";").option(member)).sep("end");
            defclass = defclass.sep("class").identifier(reserved).option(Parser.rule().sep("extends").identifier(reserved)).ast(classBody);

            param = param.identifier(reserved);
            pars = pars.ast(param).repeat(Parser.rule().sep(",").ast(param));
            paramList = paramList.sep("(").maybe(pars).sep(")");
            args = args.ast(expr).repeat(Parser.rule().sep(",").ast(expr));
            postfix = postfix.sep("(").maybe(args).sep(")");
            postfix.insertChoice(Parser.rule(typeof(Dot)).sep(".").identifier(reserved));
            def = def.sep("def").identifier(reserved).ast(paramList).ast(block);

            primary = primary.or(Parser.rule().sep("(").ast(expr).sep(")"),
                                                          Parser.rule().integer(typeof(IntegerLiteral)),
                                                          Parser.rule().identifier(typeof(Name), reserved),
                                                          Parser.rule().str(typeof(StringLiteral))).repeat(postfix);
            primary.insertChoice(Parser.rule(typeof(Lambda)).sep("lambda").ast(paramList).ast(block));

            factor = factor.or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary), primary);

            expr = expr.expression(typeof(BinaryExpr), factor, op);

            simple = simple.ast(expr).option(postfix);

            block = block.sep("begin")
                         .option(statement)
                         .repeat(Parser.rule().sep(";").option(statement))
                         .sep("end");

            statement = statement.or(
                Parser.rule(typeof(IfStmt)).sep("if").ast(expr).sep("then").ast(statement)
                                           .option(Parser.rule().sep("else").ast(statement)),
                Parser.rule(typeof(WhileStmt)).sep("while").ast(expr).sep("do").ast(statement),
                simple,
                block);

            program = program.or(defclass, def, Parser.rule().option(statement), Parser.rule(typeof(NullStmt))).sep(";");
        }

        /*  primary     : "lambda" param_list block
         *              | ( "(" expr ")" | INTEGER | IDENTIFIER | STRING ) { postfix }
         *  factor      : "-" primary | primary
         *  expr        : factor { OP factor }
         *  block       : "begin" [ statement ] { ";" [statement] } "end"
         *  simple      : expr [ postfix ]
         *  statement   : "if" expr "then" statement [ "else" statement ] 
         *              | "while" expr "do" statement 
         *              | simple 
         *              | block 
         *  param       : IDENTIFIER
         *  params      : param { "," param }
         *  param_list  : "(" [ params ] ")"
         *  def         : "def" IDENTIFIER param_list block
         *  args        : expr { "," expr }
         *  postfix     : "(" [ args ] ")"
         *  program     : [ statement | def ] ;
         */
        private void NewSpRavBNF() {
            primary = Parser.rule(typeof(PrimaryExpr));
            factor = Parser.rule();
            expr = Parser.rule();
            simple = Parser.rule(typeof(PrimaryExpr));
            block = Parser.rule(typeof(BlockStmt));
            statement = Parser.rule();
            program = Parser.rule();

            param = Parser.rule();
            pars = Parser.rule(typeof(ParameterList));
            paramList = Parser.rule();
            args = Parser.rule(typeof(Arguments));
            postfix = Parser.rule();
            def = Parser.rule(typeof(DefStmt));

            param = param.identifier(reserved);
            pars = pars.ast(param).repeat(Parser.rule().sep(",").ast(param));
            paramList = paramList.sep("(").maybe(pars).sep(")");
            args = args.ast(expr).repeat(Parser.rule().sep(",").ast(expr));
            postfix = postfix.sep("(").maybe(args).sep(")");
            def = def.sep("def").identifier(reserved).ast(paramList).ast(block);

            primary = primary.or(Parser.rule().sep("(").ast(expr).sep(")"),
                                                          Parser.rule().integer(typeof(IntegerLiteral)),
                                                          Parser.rule().identifier(typeof(Name), reserved),
                                                          Parser.rule().str(typeof(StringLiteral))).repeat(postfix);
            primary.insertChoice(Parser.rule(typeof(Lambda)).sep("lambda").ast(paramList).ast(block));

            factor = factor.or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary), primary);

            expr = expr.expression(typeof(BinaryExpr), factor, op);

            simple = simple.ast(expr).option(postfix);

            block = block.sep("begin")
                         .option(statement)
                         .repeat(Parser.rule().sep(";").option(statement))
                         .sep("end");

            statement = statement.or(
                Parser.rule(typeof(IfStmt)).sep("if").ast(expr).sep("then").ast(statement)
                                           .option(Parser.rule().sep("else").ast(statement)),
                Parser.rule(typeof(WhileStmt)).sep("while").ast(expr).sep("do").ast(statement),
                simple,
                block);

            program = program.or(def, Parser.rule().option(statement), Parser.rule(typeof(NullStmt))).sep(";");
        }

        private void SpRavBNF() {
            primary = Parser.rule(typeof(PrimaryExpr));
            factor = Parser.rule();
            expr = Parser.rule();
            simple = Parser.rule(typeof(PrimaryExpr));
            block = Parser.rule(typeof(BlockStmt));
            statement = Parser.rule();
            program = Parser.rule();

            primary = primary.or(Parser.rule().sep("(").ast(expr).sep(")"),
                                                          Parser.rule().integer(typeof(IntegerLiteral)),
                                                          Parser.rule().identifier(typeof(Name), reserved),
                                                          Parser.rule().str(typeof(StringLiteral)));

            factor = factor.or(Parser.rule(typeof(NegativeExpr)).sep("-").ast(primary), primary);

            expr = expr.expression(typeof(BinaryExpr), factor, op);

            simple = simple.ast(expr);

            block = block.sep("begin")
                         .option(statement)
                         .repeat(Parser.rule().sep(";").option(statement))
                         .sep("end");

            statement = statement.or(
                Parser.rule(typeof(IfStmt)).sep("if").ast(expr).sep("then").ast(statement)
                                           .option(Parser.rule().sep("else").ast(statement)),
                Parser.rule(typeof(WhileStmt)).sep("while").ast(expr).sep("do").ast(statement),
                simple,
                block);

            program = program.or(Parser.rule().option(statement), Parser.rule(typeof(NullStmt))).sep(";");
        }


        private void OldRavBNF() {
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
            reserved = new HashSet<string>() { ";", "begin", "end", "if", "then", "else", "while", "do", "(", ")", "class", "extends" };
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
            //NewSpRavBNF();
            RavBNFWithClass();
        }
        public ASTree Parse(Lexer lexer) {
            return program.parse(lexer);
        }
    }
}
