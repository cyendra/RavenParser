using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.LR {
    public class Project {
        public Project() { }
        public Project(string name) {
            _symbols = new List<string>();
            Name = name;
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
        private List<string> _symbols;
        public List<string> Symbols {
            get {
                return _symbols;
            }
        }
        private int _pos;
        public int Pos {
            get {
                return _pos;
            }
            set {
                _pos = value;
            }
        }
        public Project Clone() {
            Project proj = new Project();
            proj._name = _name;
            proj._symbols = _symbols;
            proj.Pos = _pos;
            return proj;
        }
        public bool SameTo(Project proj) {
            if (proj._name == _name && proj._symbols == _symbols) return true;
            return false;
        }
    }
}
