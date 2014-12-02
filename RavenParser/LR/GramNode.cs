using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.LR {
    public class GramNode : Node {
        public GramNode(string name)
            : base(NodeType.NonTerm, name) {
            _list = new List<Node>();
        }
        private List<Node> _list;
        public List<Node> Childs {
            get {
                return _list;
            }
            set {
                _list = value;
            }
        }
        public override string Description(int tab = 0) {
            StringBuilder buf = new StringBuilder();
            buf.Append(Name + "|>\r\n");
            tab += 4;
            foreach (var item in Childs) {
                for (int i = 0; i < tab; i++) buf.Append(" ");
                buf.Append(item.Description(tab));
                buf.Append("\r\n");
            }
            return buf.ToString();
        }
    }
}
