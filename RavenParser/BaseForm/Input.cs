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
using RavenParser.BaseLexer;
using RavenParser.BaseToken;
namespace RavenParser.BaseForm {
    public partial class Input : Form {
        public Input() {
            InitializeComponent();
        }

        private void runBtn_Click(object sender, EventArgs e) {
            consoleText.Clear();
            Lexer lexer = new Lexer(new StringReader(codeText.Text));
            for (Token tok = lexer.Read(); tok != Token.EOF; tok = lexer.Read()) {
                consoleText.AppendText("> " + tok.Text + "\n");
            }
            
        }
    }
}
