using Chemicals;
using System.Collections.Generic;
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

        public Substance TestSubstance;


        public Skeletone Skeletone { get => skeletone; }

        private void Start()
        {
            skeletone = new Skeletone(Heap);
            bodyParts = new List<BodyPart>();
            var parts = GetComponentsInChildren<BodyPart>();
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].Creature = this;
            }
            bodyParts.AddRange(parts);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ApplySubstance(TestSubstance);
            }
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
}