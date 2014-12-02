using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenParser.LR {
    public class State {
        public State() {
            _projects = new List<Project>();
        }
        private List<Project> _projects;
        public List<Project> Projects {
            get {
                return _projects;
            }
        }
        public delegate bool FilterLambda(Project proj);
        public List<Project> Filter(FilterLambda lambda) {
            List<Project> res = new List<Project>();
            foreach (var item in _projects) {
                if (lambda(item)) {
                    res.Add(item);
                }
            }
            return res;
        }
        public bool InState(Project proj) {
            foreach (var item in _projects) {
                if (item.SameTo(proj)) {
                    if (proj.Pos == item.Pos) {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Add(Project proj) {
            _projects.Add(proj);
        }
    }
}
