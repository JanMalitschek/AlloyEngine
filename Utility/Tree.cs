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
            Flattened = new List<Branch<T>>();
        }
        public List<Branch<T>> Flattened { get; private set; }
        public List<T> ToList(int recursionDepth = 16)
        {
            List<T> l = new List<T>();
            foreach (var b in root)
                IterateThroughTree(l, b, recursionDepth);
            return l;
        }
        private void IterateThroughTree(List<T> l, Branch<T> b, int recursionDepth)
        {
            if (recursionDepth-- <= 0) return;
            l.Add(b.Value);
            foreach (var leaf in b.Branches)
                IterateThroughTree(l, leaf, recursionDepth);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }
}
