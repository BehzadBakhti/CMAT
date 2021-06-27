using UnityEngine;
using System.Collections.Generic;


namespace Creafting
{
    public  class BuildingBlock : MonoBehaviour
    {
        private List<blockFace> edges; 
        private BuildingBlock parnet;
        private List<BuildingBlock> children;
    }
    public class Joint
    {
        private BuildingBlock _firstNode;
        private BuildingBlock _secondNode;
        private Transform _firstNodeAttachPoint;// the transform with respect to the node 1 object at wich the connection is palced
        private Transform _secondNodeAttachPOint;

        public Joint(BuildingBlock firstNode, BuildingBlock secondNode, Transform node1AP,Transform node2AP)
        {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _firstNodeAttachPoint = node1AP;
            _secondNodeAttachPOint = node2AP;
        }
    }
    public  class blockFace : MonoBehaviour
    {
        protected EdgeBoundingMaterial bounding;
        protected bool isAttached;
        private void Awake()
        {
          
        }
    }

    public class CraftedObject
    {
        List<BuildingBlock> nodes;

    }

    public enum EdgeBoundingMaterial
    {
        Glue,
        Nail,
        Magnetic,
        Hinge,
        Screw,
    }

    public enum BlockMaterial
    {
        Wood,
        Metal,

    }

}
