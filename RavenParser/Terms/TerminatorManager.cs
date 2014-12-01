using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms
{
    /// <summary>
    /// 终结符管理器
    /// </summary>
    public class TerminatorManager
    {
        public TerminatorManager()
        {
            _terminators = new SortedList<int, Dictionary<string, ITerminator>>();
            _indexDict = new Dictionary<string, int>();
        }

        private SortedList<int, Dictionary<string, ITerminator>> _terminators;
        private Dictionary<string, int> _indexDict;
     
        public delegate void TermLambda(ITerminator term);

        public bool Regist(ITerminator term)
        {
            if (_indexDict.ContainsKey(term.Name))
            {
                return false;
            }
            if (!_terminators.ContainsKey(term.Weight))
            {
                _terminators.Add(term.Weight, new Dictionary<string, ITerminator>());
            }
            _terminators[term.Weight].Add(term.Name, term);
            _indexDict.Add(term.Name, term.Weight);
            return true;
        }

        public ITerminator GetTermByName(string name)
        {
            if (_indexDict.ContainsKey(name))
            {
                int idx = _indexDict[name];
                if (!_terminators.ContainsKey(idx) || !_terminators[idx].ContainsKey(name)) return null;
                return _terminators[idx][name];
            }
            return null;
        }

        public void MapToTerm(TermLambda lambda)
        {
            foreach (var dic in  _terminators.Reverse())
            {
                foreach (var term in dic.Value)
                {
                    lambda(term.Value);
                }
            }
        }

    }
}
