using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenParser.ExToken;
namespace RavenParser.BaseParser {
    public interface ILexer {
        Token Read();
        Token Peek(int i);
    }
}
