using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms
{
    public interface INonterminal
    {
        void Add(IEnumerable<string> iter);
        int Size { get; }
        bool Remove(int idx);
        IEnumerable<string> At(int idx);
    }
}
