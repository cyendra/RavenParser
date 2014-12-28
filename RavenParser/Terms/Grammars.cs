using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RavenParser.Terms {

    public class TermGram {
        public TermGram(string name, Regex regex) {
            _name = name;
            _regex = regex;
        }
        private string _name;
        private Regex _regex;
        public string Name {
            get {
                return _name;
            }
        }
        public Regex Regex {
            get {
                return _regex;
            }
        }
    }

    public class Grammars {
        public Grammars(){
            _terminalManager = new TerminalManager();
            _nonterminalManager = new NonterminalManager();
            _termName = new HashSet<string>();
            _gramName = new HashSet<string>();
        }
        private TerminalManager _terminalManager;
        private NonterminalManager _nonterminalManager;

        public void Parsing(string grammar) {

        }

        private List<TermGram> terms;
        private List<TermGram> ignores;
        private void LoadTermRegex() {
            terms = new List<TermGram>();
            ignores = new List<TermGram>();
            _terminalManager.MapToTerm((term) => {
                Regex regex = new Regex("^" + term.Rule);
                TermGram gram = new TermGram(term.Name, regex);
                terms.Add(gram);
            });
            _terminalManager.MapToIgnore((term) => {
                Regex regex = new Regex("^" + term.Rule);
                TermGram gram = new TermGram(term.Name, regex);
                terms.Add(gram);
            });
        }

        public delegate void GramLambda(TermGram gram);
        public void MapToTerm(GramLambda lambda) {
            foreach (var item in terms) {
                lambda(item);
            }
        }
        public void MapToIgnores(GramLambda lambda) {
            foreach (var item in ignores) {
                lambda(item);
            }
        }

        private HashSet<string> _termName;
        private HashSet<string> _gramName;
        public bool InGrammars(string name) {
            if (_termName.Contains(name) || _gramName.Contains(name)) {
                return true;
            }
            return false;
        }
        public bool IsTerm(string name) {
            if (_termName.Contains(name)) {
                return true;
            }
            return false;
        }
        public bool IsGram(string name) {
            if (_gramName.Contains(name)) {
                return true;
            }
            return false;
        }

    }
}
