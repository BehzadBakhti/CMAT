using MonstersDataManagement;
using System;
using UnityEngine;

namespace Creature
{
    [Serializable]
    public class Skeletone : Tree<Bone>
    {
        Bone _root;
        public Skeletone(Bone root) : base(root)
        {
            _root = root;

            SetChilderen(Root);
        }

        internal void Elongate(Bone partRootBone, int activity)
        {

            var node= FindNode(Root, partRootBone);
            var temp = partRootBone.gameObject.transform.localScale;
            temp.z = 1+activity/100;
            partRootBone.gameObject.transform.localScale = temp;
        }

        private TreeNode<Bone> FindNode(TreeNode<Bone> node, Bone bone)
        {
            if (node.value == bone) return node;
            else
                for (int i = 0; i < node.Children.Count; i++)
                {
                    var n = FindNode(node.Children[i], bone);
                    if (n != null) return n;
                }
            return null;
        }

        private void SetChilderen(TreeNode<Bone> node)
        {
            var childeren = node.value.GetComponentsInChildren<Bone>();
            if (childeren is null || childeren.Length < 1) return;
            for (int i = 0; i < childeren.Length; i++)
            {
                if (childeren[i] == node.value) continue;
                if (childeren[i].transform.parent != node.value.transform) continue;
                var newNode = node.AddChild(childeren[i]);
                Debug.Log(newNode.value.name);
                SetChilderen(newNode);
            }
        }
    }
}
