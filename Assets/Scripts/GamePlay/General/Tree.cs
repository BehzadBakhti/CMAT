using System;
using System.Collections.Generic;

namespace MonstersDataManagement
{
    public class Tree<T>
    {
        public TreeNode<T> Root;
        public Tree(T root)
        {
            Root = new TreeNode<T>(root);
        }
        public void Depth()
        {

        }

    }

    [Serializable]
    public class TreeNode<T>
    {
        public T value;
        private TreeNode<T> Parent;
        public List<TreeNode<T>> Children;

        public TreeNode(T v)
        {
            value = v;
            Parent = null;
            Children = new List<TreeNode<T>>();
        }
        public void SetParent(TreeNode<T> p)
        {
            Parent = p;
        }

        public TreeNode<T> AddChild(T c)
        {
            var childe = new TreeNode<T>(c);
            childe.SetParent(this);
            Children.Add(childe);
            return childe;
        }
    }
}