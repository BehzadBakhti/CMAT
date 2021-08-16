using Chemicals;
using MonstersDataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Creature
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] private CreatureType _creatureType;
        [SerializeField] private float _age;

        [SerializeField] private int _mutationCount;
        [SerializeField] private List<BodyPart> bodyParts;
        private Skeletone skeletone;
        [SerializeField] Bone Heap;

        private void Start()
        {
            skeletone = new Skeletone(Heap);
        }
        public void ApplySubstance(Substance substance)
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                for (int j = 0; j < substance.elements.Count; j++)
                {
                    if (bodyParts[i].BodyPartName == substance.elements[j].bodyPart)
                    {
                        bodyParts[i].ApplySubstance(substance.elements[j], _age, _mutationCount);
                    }
                }
            }
        }

        public void Colonize(BodyPart bodyPart, int activity)
        {
            if (activity > 0)
            {
                bodyParts.Add(bodyPart);// Grow the body parts in visual 
            }
            else
            {
                var count = 0;
                BodyPart lastInstance = null;
                for (int i = 0; i < bodyParts.Count; i++)
                {
                    if (bodyPart.BodyPartName == bodyParts[i].BodyPartName)
                    {
                        count++;
                        lastInstance = bodyParts[i];
                    }
                }
                if (count > 1)
                {
                    bodyParts.Remove(lastInstance);// Shrink the body part in Visual 
                }
            }
        }
    }

   

    public enum CreatureType
    {
        Reptile,
        Flyer,
        BiPod,
        Swimmer,
    }

    [Serializable]
    public class Skeletone : Tree<Bone>
    {
        Bone _root;
        public Skeletone(Bone root) : base(root)
        {
            _root = root;

            SetChilderen(Root);
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