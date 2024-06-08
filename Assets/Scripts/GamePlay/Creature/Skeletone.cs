using MonstersDataManagement;
using System;
using System.Collections.Generic;
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
            DetachBones(Root);
        }

        internal void Elongate(Bone partRootBone, int activity)
        {

            var node = FindNode(Root, partRootBone);
            var rots = new List<Quaternion>();
            for (int i = 0; i < node.Children.Count; i++)
            {
                rots.Add(node.Children[i].value.transform.rotation);
                node.Children[i].value.transform.rotation = Quaternion.identity;
                node.Children[i].value.transform.parent = partRootBone.transform.parent;
            }
            var temp = partRootBone.gameObject.transform.localScale;


            temp.y += activity / 100f;
            partRootBone.gameObject.transform.localScale = temp;
            for (int i = 0; i < node.Children.Count; i++)
            {
                node.Children[i].value.transform.SetParent(partRootBone.transform);
                node.Children[i].value.transform.rotation = rots[i];
            }
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
                SetChilderen(newNode);
            }
        }

        private void DetachBones(TreeNode<Bone> node)
        {
            if (node.value.transform.parent is null) return;

            for (int i = 0; i < node.Children.Count; i++)
            {
                if (node.Children[i].value.transform.parent != _root.transform)
                    node.Children[i].value.transform.SetParent(_root.transform);
                DetachBones(node.Children[i]);
            }

        }
    }
}
