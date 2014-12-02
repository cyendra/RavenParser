using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.LR {
    public class Node {
        public enum NodeType {
            Term, NonTerm
        };
        private NodeType _type;
        public NodeType Type {
            get {
                return _type;
            }
            set {
                _type = value;
            }
        }
        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }
        public Node(NodeType type,string name) {
            Type = type;
            Name = name;
        }
        public virtual string Description(int tab = 0) {
            return "";
        }
    }
}
