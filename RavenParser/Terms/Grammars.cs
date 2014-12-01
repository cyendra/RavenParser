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

    }
}
