﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RavenParser.ExLexer;
using RavenParser.ExToken;
using RavenParser.ExParser;
using RavenParser.BaseParser;
using RavenParser.ExVisiter;
using RavenParser.ExEnvironment;
using RavenParser.ExException;
namespace RavenParser.ExForm {
    public partial class Input : Form {
        public Input() {
            InitializeComponent();
        }

        private void runBtn_Click(object sender, EventArgs e) {
            consoleText.Clear();
            Lexer lexer = new Lexer(new StringReader(codeText.Text));
            RavParser parser = new RavParser();
            EvalVisitor visitor = new EvalVisitor();
            visitor.DebugOption = false;
            IEnvironment env = new Natives().Enviroment(new NestedEnv());
            try {
                while (lexer.Peek(0) != Token.EOF) {
                    ASTree ast = parser.Parse(lexer);
                    //System.Console.WriteLine("  >>> " + ast.GetType().ToString() + " " + ast.ToString());
                    //consoleText.AppendText("> " + ast.ToString() + "\n");
                    ast.Accept(visitor, env);
                    consoleText.AppendText("> " + visitor.Result.ToString() + "\n");
                }
            }
            catch (ParseException ex) {
                consoleText.AppendText("> " + ex.Message + "\n");
            }       
            /*
            for (Token tok = lexer.Read(); tok != Token.EOF; tok = lexer.Read()) {
                consoleText.AppendText("> " + tok.Text + "\n");
            }
            */

            
        }
    }
}
