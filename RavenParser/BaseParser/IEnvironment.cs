using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.BaseParser {
    public interface IEnvironment {
        void Put(string name, object value);
        object Get(string name);
    }
}
