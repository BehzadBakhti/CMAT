using System;
using System.Collections.Generic;
using GamePlay.General;
using UnityEngine;

namespace Crafting
{
    [Serializable]
    public class CraftedObject : MonoBehaviour, IGraph<CraftingPart>
    {
        public IList<IGraphNode<CraftingPart>> nodes { get; set; }
        public IList<IGraphEdge<CraftingPart>> edges { get; set; }
        public void AddNode(IGraphNode<CraftingPart> node)
        {

        }

        public void AddEdge(IGraphNode<CraftingPart> node1, IGraphNode<CraftingPart> node2)
        {
            var edge = new Joint { node1 = node1, node2 = node2 };
            edges.Add(edge);
        }
    }

    [Serializable]
    public class CraftingPart : MonoBehaviour
    {
        public string partName { get; set; }
        public PartMaterial material { get; set; }
        public FaceDetection faceDetection { get; private set; }

        private void Awake()
        {
            faceDetection = GetComponent<FaceDetection>();
        }
    }

    [Serializable]
    public class Joint : IGraphEdge<CraftingPart>
    {
        public IGraphNode<CraftingPart> node1 { get; set; }
        public IGraphNode<CraftingPart> node2 { get; set; }

    }

    public enum BoundingType
    {
        Glue,
        Nail,
        Magnetic,
        Screw,
    }

    public enum PartMaterial
    {
        Wood,
        Metal,
    }
}


