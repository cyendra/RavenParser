using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.BaseParser {
    public abstract class ASTree : IEnumerable<ASTree> {
        public virtual ASTree this[int index] {
            get {
                return null;
            }
        }

        public virtual int NumChildern {
            get {
                return 0;
            }
        }
        public abstract string Location();


        #region IEnumerable<ASTree> 成员

        public virtual IEnumerator<ASTree> GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
