using System;
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
namespace RavenParser.ExForm {
    public partial class Input : Form {
        public Input() {
            InitializeComponent();
        }

        private void runBtn_Click(object sender, EventArgs e) {
            consoleText.Clear();
            Lexer lexer = new Lexer(new StringReader(codeText.Text));
            RavParser parser = new RavParser();
            try {
                while (lexer.Peek(0) != Token.EOF) {
                    ASTree ast = parser.Parse(lexer);
                    consoleText.AppendText("> " + ast.ToString() + "\n");
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
