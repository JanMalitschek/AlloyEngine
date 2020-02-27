using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy.Utility
{
    class Tree<T> : IEnumerable
    {
        public class Branch<U>
        {
            public U Value { get; set; }
            public List<Branch<U>> Branches { get; private set; }
            public int Depth { get; private set; }
            public Branch<U> Parent { 
                get
                {
                    return Parent;
                }
                private set
                {
                    Parent = value;
                    Depth = Parent == null ? 0 : (Parent.Depth + 1);
                }
            }
            public int BranchCount
            {
                get
                {
                    return Branches.Count;
                }
            }
            public Branch(U value, Branch<U> parent = null)
            {
                Value = value;
                Branches = new List<Branch<U>>();
                Parent = parent;
                Depth = Parent == null ? 0 : (Parent.Depth + 1);
            }
            public Branch<U> AddBranch(Branch<U> b)
            {
                Branches.Add(b);
                b.Parent = this;
                return b;
            }
            public bool RemoveBranch(Branch<U> b)
            {
                b.Parent = null;
                return Branches.Remove(b);
            }
            public bool RemoveBranch(int idx)
            {
                if (idx >= 0 && idx < BranchCount)
                {
                    Branches[idx].Parent = null;
                    Branches.RemoveAt(idx);
                    return true;
                }
                return false;
            }
        }
        public List<Branch<T>> root { get; private set; }
        public Tree()
        {
            root = new List<Branch<T>>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var b in root)
                yield return GetBranches(b);
        }

        private IEnumerable<Branch<T>> GetBranches(Branch<T> b)
        {
            yield return b;
            if (b.BranchCount > 0)
                foreach (var subBranch in b.Branches)
                    GetBranches(subBranch);
        }
    }
}
