using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms
{
    /// <summary>
    /// 终结符的类
    /// </summary>
    public class Terminator : ITerminator
    {
        public Terminator(string name, string rule, int weight = 0)
        {
            Name = name;
            Rule = rule;
            Weight = weight;
        }

        private string _name;
        private string _rule;
        private int _weight;

        #region ITerminator接口
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Rule
        {
            get
            {
                return _rule;
            }
            set
            {
                _rule = value;
            }
        }

        public int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }
        #endregion
    }
}
