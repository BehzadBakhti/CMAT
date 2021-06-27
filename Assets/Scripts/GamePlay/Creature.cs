using Chemicals;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Creature
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] private CreatureClass _creatureClass;
        [SerializeField] private float _age;
        [SerializeField] private int _mutationCount;
        [SerializeField] private List<BodyPart> bodyParts;
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
    public abstract class BodyPart : MonoBehaviour
    {
        public BodyPartEnum BodyPartName;
        [SerializeField] private float _length;
        [SerializeField] private float _hardness;
        [SerializeField] private float _roughness;
        [SerializeField] private Creature _creature;

        public virtual void SetLength(float length)
        {
            _length = length;
        }

        public virtual void SetHardness(float hardness)
        {
            _hardness = hardness;
        }

        public virtual void ApplySubstance(SubstanceElement substance, float age, int mutationCount)
        {
            //ToDo: the effect of the substance is affected by the "substance.dose", "age"m and "MutationCount"

            switch (substance.effect)
            {
                case SubstanceEffect.Colonizer:
                    _creature.Colonize(this, substance.activity);
                    break;
                case SubstanceEffect.Expander:
                    Expand(substance.activity);
                    break;
                case SubstanceEffect.Elongator:
                    Elongate(substance.activity);
                    break;
                case SubstanceEffect.Hardner:
                    break;
                case SubstanceEffect.Roughner:
                    break;
                default:
                    break;
            }
        }

        private void Expand(int activity)
        {

        }

        private void Elongate(int activity)
        {

        }

        private void Harden(int activity)
        {

        }

        private void Roughen(int activity)
        {

        }
    }


    public class Hand : BodyPart
    {
       
    }
    public enum CreatureClass
    {
        Reptile,
        Flyer,
        BiPod,
        Swimmer,
    }

    public enum BodyPartEnum
    {
        Head,
        Eye,
        Ear,
        Nose,
        Mouse,
        Tooth,
        Tongue,
        Neck,
        Spine,
        Foot,
        Wing,
        Tail
    }
}
