using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms {

    /// <summary>
    /// 终结符管理器
    /// </summary>
    public class TerminalManager {
        public TerminalManager() {
            _terminators = new SortedList<int, Dictionary<string, ITerminator>>();
            _indexDict = new Dictionary<string, int>();
            _ignores = new Dictionary<string, ITerminator>();
        }

        private SortedList<int, Dictionary<string, ITerminator>> _terminators;
        private Dictionary<string, int> _indexDict;
        private Dictionary<string, ITerminator> _ignores;

        public delegate void TermLambda(ITerminator term);

        public bool Regist(ITerminator term) {
            if (_indexDict.ContainsKey(term.Name)) {
                return false;
            }
            if (term.Weight < 0) return false;
            if (!_terminators.ContainsKey(term.Weight)) {
                _terminators.Add(term.Weight, new Dictionary<string, ITerminator>());
            }
            _terminators[term.Weight].Add(term.Name, term);
            _indexDict.Add(term.Name, term.Weight);
            return true;
        }

        public ITerminator GetTermByName(string name) {
            if (_indexDict.ContainsKey(name)) {
                int idx = _indexDict[name];
                if (idx < 0) {
                    if (!_ignores.ContainsKey(name)) {
                        return null;
                    }
                    return _ignores[name];
                }
                if (!_terminators.ContainsKey(idx) || !_terminators[idx].ContainsKey(name)) {
                    return null;
                }
                return _terminators[idx][name];
            }
            return null;
        }

        public bool RemoveTermByName(string name) {
            if (!_indexDict.ContainsKey(name)) {
                return false;
            }
            int idx = _indexDict[name];
            if (idx < 0) {
                if (_ignores.ContainsKey(name)) {
                    _ignores.Remove(name);
                    _indexDict.Remove(name);
                }
            }
            else {
                if (_terminators.ContainsKey(idx) && _terminators[idx].ContainsKey(name)) {
                    _terminators[idx].Remove(name);
                }
            }
            _indexDict.Remove(name);
            return true;
        }

        public void MapToTerm(TermLambda lambda) {
            foreach (var dic in _terminators.Reverse()) {
                foreach (var term in dic.Value) {
                    lambda(term.Value);
                }
            }
        }

        public void MapToIgnore(TermLambda lambda) {
            foreach (var term in _ignores) {
                lambda(term.Value);
            }
        }

        public bool Ignore(ITerminator term) {
            if (_indexDict.ContainsKey(term.Name)) {
                return false;
            }
            term.Weight = -1;
            _ignores.Add(term.Name, term);
            _indexDict.Add(term.Name, -1);
            return true;
        }

        public bool Contains(string name) {
            if (!_indexDict.ContainsKey(name)) {
                return false;
            }
            return true;
        }

        public bool IsIgnore(string name) {
            if (!Contains(name)) return false;
            int idx = _indexDict[name];
            if (idx < 0) return true;
            return false;
        }

        public bool IsIgnore(ITerminator term) {
            return IsIgnore(term.Name);
        }

        public bool IsRegist(string name) {
            if (!Contains(name)) return false;
            int idx = _indexDict[name];
            if (idx >= 0) return true;
            return false;
        }

        public bool IsRegist(ITerminator term) {
            return IsRegist(term.Name);
        }
    }

}
