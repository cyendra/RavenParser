using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.BaseParser {
    public class Token {
        public static readonly Token EOF = new Token(-1);
        public static readonly string EOL = "\\n";
        
        private int lineNumber;
        public virtual int LineNumber {
            get {
                return lineNumber;
            }
        }
        public Token(int line) {
            lineNumber = line;
        }

        public virtual string Text {
            get {
                return "";
            }
        }
        public virtual bool IsIdentifier {
            get {
                return false;
            }
        }
        public virtual bool IsFloat {
            get {
                return false;
            }
        }
        public virtual bool IsInteger {
            get {
                return false;
            }
        }
        public virtual bool IsString {
            get {
                return false;
            }
        }
        public virtual int IntegerNumber {
            get {
                return 0;
            }
        }
        public virtual double FloatNumber {
            get {
                return 0;
            }
        }
        
    }
}
