using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.LR {
    public class TermNode : Node {
        public TermNode(string name, string text)
            : base(Node.NodeType.Term, name) {
            Text = text;
        }

        private string _text;
        public string Text {
            get {
                return _text;
            }
            set {
                _text = value;
            }
        }
        public override string Description(int tab = 0) {
            return Name + ":" + Text;
        }
    }
}
