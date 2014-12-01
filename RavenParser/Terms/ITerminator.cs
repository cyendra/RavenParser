using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.Terms
{
    /// <summary>
    /// 终结符的接口
    /// </summary>
    public interface ITerminator
    {
        string Name { get; set; }
        string Rule { get; set; }
        int Weight { get; set; }
    }
}
